#include "config.h"
#include <WiFi101.h>
WiFiSSLClient client;

void postEvent(const char* content)
{
  if (client.connect(HTTPS_HOST, HTTPS_PORT)) {
    char buffer[192];
    snprintf(buffer, 192, "DeviceId=%s&%s", HTTPS_DEVICE_ID, content);
    client.print("POST ");
    client.print(HTTPS_URL);
#ifdef HTTPS_CODE_KEY
    client.print("?");
    client.print(HTTPS_CODE_KEY);
    client.print("=");
    client.print(HTTPS_CODE_VALUE);
#endif
    client.println(" HTTP/1.1");
    client.print("Host: ");
    client.println(HTTPS_HOST);
    client.print("Content-Length: ");
    client.println(strlen(buffer));
    client.println("Content-Type: application/x-www-form-urlencoded");
    client.println("Connection: close");
    client.println();
    client.println(buffer);
  }
}

void deviceTeardown()
{
  
}

void deviceProcess(int ms)
{
  delay(ms);
}

void flowSend(int number, int reportingSpeed, long pulses)
{
  char buffer[128];
  snprintf(buffer, 128, "Event=flow&Number=%i&Speed=%i&Pulses=%l", number, reportingSpeed, pulses);
  postEvent(buffer);
}

void weightSend(int number, int weight)
{
  char buffer[128];
  snprintf(buffer, 128, "Event=weight&Number=%i&Weight=%i", number, weight);
  postEvent(buffer);
}

void temperatureSend(const char* sensor, float temperature)
{
  char buffer[128];
  snprintf(buffer, 128, "Event=temp&Sensor=%s&Temp=%f", sensor, temperature);
  postEvent(buffer);
}

void errorSend(const char* error)
{
  char buffer[128];
  snprintf(buffer, 128, "Event=error&Message=%s", error);
  postEvent(buffer);
}

