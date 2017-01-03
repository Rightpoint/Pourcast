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

#include "init.h"
#include "TapMonitor.h"
#include "config.h"

static char ssid[] = WIFI_SSID;
static char pass[] = WIFI_PASSWORD;
static char connectionString[] = IOT_CONNECTIONSTRING;

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
}

void loop() {
  deviceProcess(100);
}


