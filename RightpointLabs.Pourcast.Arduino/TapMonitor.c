#include "AzureIoTHub.h"
#include "sdk/schemaserializer.h"

#define MAX_DEVICE_ID_SIZE  20

/* CODEFIRST_OK is the new name for IOT_AGENT_OK. The follow #ifndef helps during the name migration. Remove it when the migration ends. */
#ifndef  IOT_AGENT_OK
#define  IOT_AGENT_OK CODEFIRST_OK
#endif // ! IOT_AGENT_OK


// Define the Model
BEGIN_NAMESPACE(Pourcast);

DECLARE_STRUCT(SystemProperties,
               ascii_char_ptr, DeviceID,
               _Bool, Enabled
              );

DECLARE_STRUCT(DeviceProperties,
               ascii_char_ptr, DeviceID,
               _Bool, HubEnabledState
              );

DECLARE_MODEL(KegState,
              /* Event data (temperature, external temperature and humidity) */
              WITH_DATA(ascii_char_ptr, DeviceId),

              WITH_DATA(int, KegNumber),
              WITH_DATA(long, TimeSinceLastData),
              WITH_DATA(int, Weight),
              WITH_DATA(long, Pulses),
              WITH_DATA(int, ReportingSpeed),

              WITH_DATA(ascii_char_ptr, TemperatureSensor),
              WITH_DATA(double, Temperature),
              
              WITH_DATA(ascii_char_ptr, Error),

              /* Device Info - This is command metadata + some extra fields */
              WITH_DATA(ascii_char_ptr, ObjectType),
              WITH_DATA(_Bool, IsSimulatedDevice),
              WITH_DATA(ascii_char_ptr, Version),
              WITH_DATA(DeviceProperties, DeviceProperties),
              WITH_DATA(ascii_char_ptr_no_quotes, Commands) //,

              /* Commands implemented by the device */
             );

END_NAMESPACE(Pourcast);
  

static void sendMessage(IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle, const unsigned char* buffer, size_t size)
{
  IOTHUB_MESSAGE_HANDLE messageHandle = IoTHubMessage_CreateFromByteArray(buffer, size);
  if (messageHandle == NULL)
  {
    LogInfo("unable to create a new IoTHubMessage\r\n");
  }
  else
  {
    if (IoTHubClient_LL_SendEventAsync(iotHubClientHandle, messageHandle, NULL, NULL) != IOTHUB_CLIENT_OK)
    {
      LogInfo("failed to hand over the message to IoTHubClient");
    }
    else
    {
      LogInfo("IoTHubClient accepted the message for delivery\r\n");
    }

    IoTHubMessage_Destroy(messageHandle);
  }
  free((void*)buffer);
}

static size_t GetDeviceId(const char* connectionString, char* deviceID, size_t size)
{
  size_t result;
  const char* runStr = connectionString;
  char ustate = 0;
  char* start = NULL;

  if (runStr == NULL)
  {
    result = 0;
  }
  else
  {
    while (*runStr != '\0')
    {
      if (ustate == 0)
      {
        if (strncmp(runStr, "DeviceId=", 9) == 0)
        {
          runStr += 9;
          start = runStr;
        }
        ustate = 1;
      }
      else
      {
        if (*runStr == ';')
        {
          if (start == NULL)
          {
            ustate = 0;
          }
          else
          {
            break;
          }
        }
        runStr++;
      }
    }

    if (start == NULL)
    {
      result = 0;
    }
    else
    {
      result = runStr - start;
      if (deviceID != NULL)
      {
        for (size_t i = 0; ((i < size - 1) && (start < runStr)); i++)
        {
          *deviceID++ = *start++;
        }
        *deviceID = '\0';
      }
    }
  }

  return result;
}

/*this function "links" IoTHub to the serialization library*/
static IOTHUBMESSAGE_DISPOSITION_RESULT IoTHubMessage(IOTHUB_MESSAGE_HANDLE message, void* userContextCallback)
{
  IOTHUBMESSAGE_DISPOSITION_RESULT result;
  const unsigned char* buffer;
  size_t size;
  if (IoTHubMessage_GetByteArray(message, &buffer, &size) != IOTHUB_MESSAGE_OK)
  {
    LogInfo("unable to IoTHubMessage_GetByteArray\r\n");
    result = EXECUTE_COMMAND_ERROR;
  }
  else
  {
    /*buffer is not zero terminated*/
    char* temp = malloc(size + 1);
    if (temp == NULL)
    {
      LogInfo("failed to malloc\r\n");
      result = EXECUTE_COMMAND_ERROR;
    }
    else
    {
      EXECUTE_COMMAND_RESULT executeCommandResult;

      memcpy(temp, buffer, size);
      temp[size] = '\0';
      executeCommandResult = EXECUTE_COMMAND(userContextCallback, temp);
      result =
        (executeCommandResult == EXECUTE_COMMAND_ERROR) ? IOTHUBMESSAGE_ABANDONED :
        (executeCommandResult == EXECUTE_COMMAND_SUCCESS) ? IOTHUBMESSAGE_ACCEPTED :
        IOTHUBMESSAGE_REJECTED;
      free(temp);
    }
  }
  return result;
}

IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle;
KegState* kegState;
char deviceId[MAX_DEVICE_ID_SIZE];

void deviceSetup(const char* connectionString)
{
  srand((unsigned int)time(NULL));
  if (serializer_init(NULL) != SERIALIZER_OK)
  {
    LogInfo("Failed on serializer_init\r\n");
  }
  else
  {
    iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(connectionString, HTTP_Protocol);
    if (iotHubClientHandle == NULL)
    {
      LogInfo("Failed on IoTHubClient_CreateFromConnectionString\r\n");
    }
    else
    {
#ifdef MBED_BUILD_TIMESTAMP
      // For mbed add the certificate information
      if (IoTHubClient_LL_SetOption(iotHubClientHandle, "TrustedCerts", certificates) != IOTHUB_CLIENT_OK)
      {
        LogInfo("failure to set option \"TrustedCerts\"\r\n");
      }
#endif // MBED_BUILD_TIMESTAMP

      kegState = CREATE_MODEL_INSTANCE(Pourcast, KegState);
      if (kegState == NULL)
      {
        LogInfo("Failed on CREATE_MODEL_INSTANCE\r\n");
      }
      else
      {
        STRING_HANDLE commandsMetadata;

        if (IoTHubClient_LL_SetMessageCallback(iotHubClientHandle, IoTHubMessage, kegState) != IOTHUB_CLIENT_OK)
        {
          LogInfo("unable to IoTHubClient_SetMessageCallback\r\n");
        }
        else
        {
          if (GetDeviceId(connectionString, deviceId, MAX_DEVICE_ID_SIZE) > 0)
          {
            LogInfo("deviceId=%s", deviceId);
          }

          /* send the device info upon startup so that the cloud app knows
            what commands are available and the fact that the device is up */
          kegState->ObjectType = "DeviceInfo";
          kegState->IsSimulatedDevice = false;
          kegState->Version = "1.0";
          kegState->DeviceProperties.HubEnabledState = true;
          kegState->DeviceProperties.DeviceID = (char*)deviceId;

          commandsMetadata = STRING_new();
          if (commandsMetadata == NULL)
          {
            LogInfo("Failed on creating string for commands metadata\r\n");
          }
          else
          {
            /* Serialize the commands metadata as a JSON string before sending */
            if (SchemaSerializer_SerializeCommandMetadata(GET_MODEL_HANDLE(Pourcast, KegState), commandsMetadata) != SCHEMA_SERIALIZER_OK)
            {
              LogInfo("Failed serializing commands metadata\r\n");
            }
            else
            {
              unsigned char* buffer;
              size_t bufferSize;
              kegState->Commands = (char*)STRING_c_str(commandsMetadata);

              /* Here is the actual send of the Device Info */
              if (SERIALIZE(&buffer, &bufferSize, kegState->ObjectType, kegState->Version, kegState->IsSimulatedDevice, kegState->DeviceProperties, kegState->Commands) != IOT_AGENT_OK)
              {
                LogInfo("Failed serializing\r\n");
              }
              else
              {
                sendMessage(iotHubClientHandle, buffer, bufferSize);
              }

            }

            STRING_delete(commandsMetadata);
          }

          kegState->DeviceId = (char*)deviceId;
        }
      }
    }
  }
}

void deviceProcess(int delay) {
  IoTHubClient_LL_DoWork(iotHubClientHandle);
  ThreadAPI_Sleep(delay);
}

void deviceTeardown() {
  DESTROY_MODEL_INSTANCE(kegState);
  IoTHubClient_LL_Destroy(iotHubClientHandle);
  serializer_deinit();
}

void deviceSend(long timeSinceLastData, int weight, long pulses, int kegNumber, int reportingSpeed) {
  kegState->TimeSinceLastData = timeSinceLastData;
  kegState->Weight = weight;
  kegState->Pulses = pulses;
  kegState->KegNumber = kegNumber;
  kegState->ReportingSpeed = reportingSpeed;
  
  unsigned char*buffer;
  size_t bufferSize;

  int serializeResult = weight == -1 ?
    SERIALIZE(&buffer, &bufferSize, kegState->DeviceId, kegState->TimeSinceLastData, kegState->Pulses, kegState->KegNumber, kegState->ReportingSpeed) :
    SERIALIZE(&buffer, &bufferSize, kegState->DeviceId, kegState->TimeSinceLastData, kegState->Weight, kegState->Pulses, kegState->KegNumber, kegState->ReportingSpeed);

  if (serializeResult != IOT_AGENT_OK)
  {
    LogInfo("Failed sending sensor value %s %i %i %i %s %i\r\n", kegState->DeviceId, kegState->TimeSinceLastData, kegState->Weight, kegState->Pulses, kegState->KegNumber, kegState->ReportingSpeed);
  }
  else
  {
    sendMessage(iotHubClientHandle, buffer, bufferSize);
    LogInfo("Succeeded sending sensor value %s %i %i %i %s %i\r\n", kegState->DeviceId, kegState->TimeSinceLastData, kegState->Weight, kegState->Pulses, kegState->KegNumber, kegState->ReportingSpeed);
  }
}

void temperatureSend(const char* sensor, float temperature) {
  kegState->TemperatureSensor = sensor;
  kegState->Temperature = temperature;
  
  unsigned char*buffer;
  size_t bufferSize;

  if (SERIALIZE(&buffer, &bufferSize, kegState->DeviceId, kegState->TemperatureSensor, kegState->Temperature) != IOT_AGENT_OK)
  {
    LogInfo("Failed sending temperature value %s %s %f\r\n", kegState->DeviceId, kegState->TemperatureSensor, kegState->Temperature);
  }
  else
  {
    sendMessage(iotHubClientHandle, buffer, bufferSize);
    LogInfo("Succeeded sending temperature value %s %s $f\r\n", kegState->DeviceId, kegState->TemperatureSensor, kegState->Temperature);
  }
}

void sendError(const char* error) {
  kegState->Error = error;
  
  unsigned char*buffer;
  size_t bufferSize;

  if (SERIALIZE(&buffer, &bufferSize, kegState->DeviceId, kegState->Error) != IOT_AGENT_OK)
  {
    LogInfo("Failed sending error %s %s\r\n", kegState->DeviceId, kegState->Error);
  }
  else
  {
    sendMessage(iotHubClientHandle, buffer, bufferSize);
    LogInfo("Succeeded sending error %s %s\r\n", kegState->DeviceId, kegState->Error);
  }
}

