// starting from https://azure.microsoft.com/en-us/resources/samples/iot-hub-c-m0wifi-getstartedkit/

//#define LOOP_DEBUG
//#define PULSE_DEBUG

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

#include <OneWire.h>

#include "init.h"
#include "TapMonitor.h"
#include "Tap.h"
#include "config.h"

static char ssid[] = WIFI_SSID;
static char pass[] = WIFI_PASSWORD;
static char connectionString[] = IOT_CONNECTIONSTRING;
static byte sensor1[] = { 0x28, 0x8A, 0x8E, 0x2A, 0x06, 0x00, 0x00, 0xE8 };
static byte sensor2[] = { 0x28, 0xA9, 0xD2, 0xFA, 0x05, 0x00, 0x00, 0x56 };
static char kegId1[] = "535c61a951aa0405287989ec";
static char kegId2[] = "537d28db51aa04289027cde5";
OneWire oneWire(6);
Tap* tap1;
Tap* tap2;

#define WINC_CS   8
#define WINC_IRQ  7
#define WINC_RST  4
#define WINC_EN   2

// Setup the WINC1500 connection with the pins above and the default hardware SPI.
Adafruit_WINC1500 WiFi(WINC_CS, WINC_IRQ, WINC_RST);

int status = WL_IDLE_STATUS;

// the setup function runs once when you press reset or power the board
void setup() {
  pinMode(WINC_EN, OUTPUT);
  digitalWrite(WINC_EN, HIGH);
    
  initWifi(ssid, pass);
  initTime();

  deviceSetup(connectionString);

  int pin1 = 9;
  int int1 = digitalPinToInterrupt(pin1);
  int pin2 = 10;
  int int2 = digitalPinToInterrupt(pin2);


  pinMode(pin1, INPUT);
  tap1 = new Tap(&oneWire, sensor1, kegId1, 1);
  attachInterrupt(int1, tap1Pulse, RISING);
  
  pinMode(pin2, INPUT);
  tap2 = new Tap(&oneWire, sensor2, kegId2, 1);
  attachInterrupt(int2, tap2Pulse, RISING);


  Serial.print("Mapping ");
  Serial.print(pin1);
  Serial.print(" to ");
  Serial.println(int1);

  Serial.print("Mapping ");
  Serial.print(pin2);
  Serial.print(" to ");
  Serial.println(int2);
}

// handle interrupt pulses from the taps
void tap1Pulse() {
#ifdef PULSE_DEBUG
  Serial.println("Handling 1");
#endif
  tap1->HandlePulse();
}
void tap2Pulse() {
#ifdef PULSE_DEBUG
  Serial.println("Handling 2");
#endif
  tap2->HandlePulse();
}


void loop() {
  int cycle = 0;
  
  const int goal = 100;
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
    
    tap1->Loop(cycle);
    tap2->Loop(cycle);

    if(cycle % 100 == 0) {
      Serial.println("ALIVE");
    }

    cycle = (cycle + 1) % 6000;

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
