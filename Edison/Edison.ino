#include "Arduino.h"

#include <Dhcp.h>
#include <Dns.h>
#include <WiFi.h>
#include <WiFiClient.h>
#include <WiFiServer.h>
#include <WiFiUdp.h>

#include "Tap.h"
#include "MultiReporter.h"
#include "LEDReporter.h"
#include "SerialReporter.h"
#include "NetworkReporter.h"

Tap* tap1;
Tap* tap2;
NetworkRequester* http;

// handle interrupt pulses from the taps
void tap1Pulse() {
  tap1->HandlePulse();
}
void tap2Pulse() {
  tap2->HandlePulse();
}

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);

  Serial.println("Starting");

  // check for the presence of the shield:
  if (WiFi.status() == WL_NO_SHIELD) {
    Serial.println("WiFi shield not present"); 
    // don't continue:
    while(true);
  } 

  String fv = WiFi.firmwareVersion();
  if( fv != "1.1.0" )
    Serial.println("Please upgrade the firmware");

  int status = WiFi.status();
  char* ssid = "";
  const char* pass = "";

  // attempt to connect to Wifi network:
  while (status != WL_CONNECTED) { 
    Serial.print("Wifi Status: ");
    Serial.println(status);
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:    
    status = WiFi.begin(ssid, pass);

    // wait 10 seconds for connection:
    for(int i=0; i<10; i++) {
      status = WiFi.status();
      if(status == WL_CONNECTED)
        break;
      delay(1000);
    }
  } 
  Serial.println("Connected to wifi");
  http = new NetworkRequester("pourcast.labs.rightpoint.com", 80, 11);

  String wifiStatus = getWifiStatus();
  Serial.println(wifiStatus);
  http->LogMessage(wifiStatus);

  tap1 = new Tap(new MultiReporter(new SerialReporter(1), new LEDReporter(9), new NetworkReporter(http, "535c61a951aa0405287989ec")));
  attachInterrupt(2, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new SerialReporter(2), new LEDReporter(10), new NetworkReporter(http, "537d28db51aa04289027cde5")));
  attachInterrupt(3, tap2Pulse, RISING);
}

void startupDelay() {
  http->LogMessage("Begining startup delay");
  delay(2000);
  long tap1Pulses = tap1->Clear();
  long tap2Pulses = tap2->Clear();
  char buf[256];
  snprintf(buf, 256, "Startup delay complete (ignored %li and %li)", tap1Pulses, tap2Pulses);
  http->LogMessage(buf);
}

// main loop - once a second, check the accumulated pulse counts and send the START/STOP/CONTINUE/IGNORE message as necessary
// send an ALIVE message every loop to assist debugging
void loop() {
  startupDelay();
    
  int cycle = 0;  
  http->LogMessage("Primary loop initialized");
  while(true) {
    tap1->Loop(cycle);
    tap2->Loop(cycle);

    if(cycle % 6000 == 0) {
      String wifiStatus = getWifiStatus();
      Serial.println(wifiStatus);
      http->LogMessage(wifiStatus);
    }
    if(cycle % 600 == 0) {
      http->Heartbeat();
    } else if(cycle % 100 == 0) {
      Serial.println("ALIVE");
    }
    cycle = (cycle + 1) % 6000;
    delay(100);
  }
}

String getWifiStatus() {
  String prefix = "SSID: ";
  IPAddress ip = WiFi.localIP();
  char buf[0x20];
  snprintf(buf, sizeof(buf), "%3d.%3d.%3d.%3d", ip[0], ip[1], ip[2], ip[3]);

  return prefix + WiFi.SSID() + ", IP Address: " + buf + ", RSSI: " + WiFi.RSSI() + " dBm";
}

