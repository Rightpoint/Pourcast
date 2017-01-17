// starting from https://azure.microsoft.com/en-us/resources/samples/iot-hub-c-m0wifi-getstartedkit/

#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <time.h>
#include <sys/time.h>
#include <SPI.h>

#include <Adafruit_WINC1500.h>
#include <Adafruit_WINC1500Client.h>
#include <Adafruit_WINC1500Server.h>
#include <Adafruit_WINC1500SSLClient.h>
#include <Adafruit_WINC1500Udp.h>

#include <AzureIoTHub.h>
#include <AzureIoTUtility.h>
#include <AzureIoTProtocol_HTTP.h>

#include <Q2HX711.h>
#include "init.h"
#include "TapMonitor.h"
#include "Tap.h"
#include "config.h"

static char ssid[] = WIFI_SSID;
static char pass[] = WIFI_PASSWORD;
static char connectionString[] = IOT_CONNECTIONSTRING;

#ifdef ONEWIRE_PIN
#include <OneWire.h>
OneWire oneWire(ONEWIRE_PIN);
#include "temp.h"
#endif

#ifdef KEG1_PIN
#ifdef KEG1_WEIGHT_PIN
Q2HX711 weight1(KEG1_WEIGHT_PIN, KEG1_WEIGHT_CLOCK_PIN);
Tap tap1(1, &weight1);
#else
Tap tap1(1);
#endif
#endif

#ifdef KEG2_PIN
#ifdef KEG2_WEIGHT_PIN
Q2HX711 weight2(KEG2_WEIGHT_PIN, KEG2_WEIGHT_CLOCK_PIN);
Tap tap2(2, &weight2);
#else
Tap tap2(2);
#endif
#endif

#ifdef KEG3_PIN
#ifdef KEG3_WEIGHT_PIN
Q2HX711 weight3(KEG3_WEIGHT_PIN, KEG3_WEIGHT_CLOCK_PIN);
Tap tap3(3, &weight3);
#else
Tap tap3(3);
#endif
#endif

#ifdef KEG4_PIN
#ifdef KEG4_WEIGHT_PIN
Q2HX711 weight4(KEG4_WEIGHT_PIN, KEG4_WEIGHT_CLOCK_PIN);
Tap tap4(4, &weight4);
#else
Tap tap4(4);
#endif
#endif

#define WINC_CS   8
#define WINC_IRQ  7
#define WINC_RST  4
#define WINC_EN   2

// Setup the WINC1500 connection with the pins above and the default hardware SPI.
Adafruit_WINC1500 WiFi(WINC_CS, WINC_IRQ, WINC_RST);

#ifdef ONEWIRE_PIN
int num_temp_sensors = 0;
byte temp_sensors[MAX_TEMP_SENSORS][8];
#endif

int status = WL_IDLE_STATUS;

// the setup function runs once when you press reset or power the board
void setup() {
  pinMode(WINC_EN, OUTPUT);
  digitalWrite(WINC_EN, HIGH);
    
  initWifi(ssid, pass);
  initTime();

  deviceSetup(connectionString);

#ifdef ONEWIRE_PIN
  byte addr[8];
  char buffer[128];
  oneWire.reset_search();
  while(oneWire.search(addr)) {
    sprintf(buffer, "%02x%02x%02x%02x%02x%02x%02x%02x%02x", addr[0], addr[1], addr[2], addr[3], addr[4], addr[5], addr[6], addr[7]);
    if ( OneWire::crc8( addr, 7) != addr[7]) {
      Serial.print("CRC is not valid! ");
      Serial.println(buffer);
      continue;
    }
    Serial.print("Found One-wire device");
    Serial.println(buffer);
    if(addr[0] == 0x28) {
      if(num_temp_sensors < MAX_TEMP_SENSORS) {
        memcpy(temp_sensors[num_temp_sensors], addr, 8);
        num_temp_sensors++;
        Serial.println("... added");
      } else {
        Serial.print("Too many temperature sensors found - only using first ");
        Serial.println(num_temp_sensors);
      }
    } else {
        Serial.print("Unknown OneWire device family for ");
        Serial.println(buffer);
    }
  }
#endif

#ifdef KEG1_PIN
  pinMode(KEG1_PIN, INPUT);
  attachInterrupt(digitalPinToInterrupt(KEG1_PIN), tap1Pulse, RISING);
  Serial.print("Mapping ");
  Serial.print(KEG1_PIN);
  Serial.print(" to ");
  Serial.print(digitalPinToInterrupt(KEG1_PIN));
  Serial.println(" for 1");
#ifdef KEG1_WEIGHT_PIN
  pinMode(KEG1_WEIGHT_PIN, INPUT);
  pinMode(KEG1_WEIGHT_CLOCK_PIN, OUTPUT);
#endif
#endif

#ifdef KEG2_PIN
  pinMode(KEG2_PIN, INPUT);
  attachInterrupt(digitalPinToInterrupt(KEG2_PIN), tap2Pulse, RISING);
  Serial.print("Mapping ");
  Serial.print(KEG2_PIN);
  Serial.print(" to ");
  Serial.print(digitalPinToInterrupt(KEG2_PIN));
  Serial.println(" for 2");
#ifdef KEG2_WEIGHT_PIN
  pinMode(KEG2_WEIGHT_PIN, INPUT);
  pinMode(KEG2_WEIGHT_CLOCK_PIN, OUTPUT);
#endif
#endif

#ifdef KEG3_PIN
  pinMode(KEG3_PIN, INPUT);
  attachInterrupt(digitalPinToInterrupt(KEG3_PIN), tap3Pulse, RISING);
  Serial.print("Mapping ");
  Serial.print(KEG3_PIN);
  Serial.print(" to ");
  Serial.print(digitalPinToInterrupt(KEG3_PIN));
  Serial.println(" for 3");
#ifdef KEG3_WEIGHT_PIN
  pinMode(KEG3_WEIGHT_PIN, INPUT);
  pinMode(KEG3_WEIGHT_CLOCK_PIN, OUTPUT);
#endif
#endif

#ifdef KEG4_PIN
  pinMode(KEG4_PIN, INPUT);
  attachInterrupt(digitalPinToInterrupt(KEG4_PIN), tap4Pulse, RISING);
  Serial.print("Mapping ");
  Serial.print(KEG4_PIN);
  Serial.print(" to ");
  Serial.print(digitalPinToInterrupt(KEG4_PIN));
  Serial.println(" for 4");
#ifdef KEG4_WEIGHT_PIN
  pinMode(KEG4_WEIGHT_PIN, INPUT);
  pinMode(KEG4_WEIGHT_CLOCK_PIN, OUTPUT);
#endif
#endif

  
  #ifdef ONEWIRE_PIN
  requestAllTemps();
  delay(1000);
  #endif
}

// handle interrupt pulses from the taps
#ifdef KEG1_PIN
void tap1Pulse() {
#ifdef PULSE_DEBUG
  Serial.println("Handling 1");
#endif
  tap1.HandlePulse();
}
#endif
#ifdef KEG2_PIN
void tap2Pulse() {
#ifdef PULSE_DEBUG
  Serial.println("Handling 2");
#endif
  tap2.HandlePulse();
}
#endif
#ifdef KEG3_PIN
void tap3Pulse() {
#ifdef PULSE_DEBUG
  Serial.println("Handling 3");
#endif
  tap3.HandlePulse();
}
#endif
#ifdef KEG4_PIN
void tap4Pulse() {
#ifdef PULSE_DEBUG
  Serial.println("Handling 4");
#endif
  tap4.HandlePulse();
}
#endif

#ifdef ONEWIRE_PIN
void requestAllTemps() {
  for(int i=0; i<num_temp_sensors; i++) {
    requestTempMeasurement(&oneWire, temp_sensors[i]);
  }
}

void sendAllTemps() {
  for(int i=0; i<num_temp_sensors; i++) {
    byte *addr = temp_sensors[i];
    float temperature = readTemp(&oneWire, addr);
    char buffer[128];
    sprintf(buffer, "%02x%02x%02x%02x%02x%02x%02x%02x%02x", addr[0], addr[1], addr[2], addr[3], addr[4], addr[5], addr[6], addr[7]);
    temperatureSend(buffer, temperature);
  }
}
#endif


void loop() {
  int cycle = 0;
  
  const int goal = LOOP_GOAL_MS;
  const int requestTempsOnLoopNumber = (SEND_TEMPERATURE_EVERY - 1000 / LOOP_GOAL_MS);
  int offset = 0;
  int target = goal;

  while(true) {
    int start = millis();

    target = goal;
    if(offset > goal/2) {
      target -= goal/2;
    } else if(offset > 0) {
      target -= offset;
    } else if(offset < -goal/2) {
      target += goal/2;
    } else if(offset < 0) {
      target += offset;
    }

    #ifdef KEG1_PIN
    tap1.Loop(cycle);
    #endif
    #ifdef KEG2_PIN
    tap2.Loop(cycle);
    #endif
    #ifdef KEG3_PIN
    tap3.Loop(cycle);
    #endif
    #ifdef KEG4_PIN
    tap4.Loop(cycle);
    #endif

    #ifdef ONEWIRE_PIN
    if(cycle % SEND_TEMPERATURE_EVERY == requestTempsOnLoopNumber) {
      requestAllTemps();
    }
    if(cycle % SEND_TEMPERATURE_EVERY == 0) {
      sendAllTemps();
    }
    #endif

    if(cycle % 100 == 0) {
      Serial.println("ALIVE");
    }

    cycle = (cycle + 1) % MAX_LOOP_COUNT;

    deviceProcess(target);
    int done = millis();

#ifdef LOOP_DEBUG
    Serial.print(goal);
    Serial.print(" ");
    Serial.print(offset);
    Serial.print(" -> ");
    Serial.print(target);
    Serial.print(" = ");
    Serial.print(done-start);
#endif
    
    offset += (done-start)-goal;

#ifdef LOOP_DEBUG
    Serial.print(" -> ");
    Serial.println(offset);
#endif
  }
}
