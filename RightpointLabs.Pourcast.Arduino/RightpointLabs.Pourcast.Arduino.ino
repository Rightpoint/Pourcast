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
#include <AzureIoTProtocol_MQTT.h>

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

  tap1 = new Tap(&oneWire, sensor1, "535c61a951aa0405287989ec", 1);
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(&oneWire, sensor2, "537d28db51aa04289027cde5", 1);
  attachInterrupt(1, tap2Pulse, RISING);
}

// handle interrupt pulses from the taps
void tap1Pulse() {
  tap1->HandlePulse();
}
void tap2Pulse() {
  tap2->HandlePulse();
}


void loop() {
  int cycle = 0;  
  while(true) {
    tap1->Loop(cycle);
    tap2->Loop(cycle);

    if(cycle % 100 == 0) {
      Serial.println("ALIVE");
    }

    cycle = (cycle + 1) % 6000;
    deviceProcess(100);
  }
}


