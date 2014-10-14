#include "Arduino.h"
#include <WiFi.h>

#include "Tap.h"
#include "MultiReporter.h"
#include "LEDReporter.h"
#include "SerialReporter.h"
#include "NetworkReporter.h"

Tap* tap1;
Tap* tap2;
NetworkRequester* http;

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);

  byte mac[] = { 0xfc,0xc2,0xde,0x2f,0x58,0xe3 };
  
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
  
  // attempt to connect to Wifi network:
  while (status != WL_CONNECTED) { 
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:    
    status = WiFi.begin(ssid, pass);
  
    // wait 10 seconds for connection:
    delay(10000);
  } 
  Serial.println("Connected to wifi");
  printWifiStatus();
  
  http = new NetworkRequester("pourcast.labs.rightpoint.com", 80, 9);
  tap1 = new Tap(new MultiReporter(new SerialReporter(1), new LEDReporter(10), new NetworkReporter(http, "535c61a951aa0405287989ec")));
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new SerialReporter(2), new LEDReporter(11), new NetworkReporter(http, "537d28db51aa04289027cde5")));
  attachInterrupt(1, tap2Pulse, RISING);
}

void printWifiStatus() {
  // print the SSID of the network you're attached to:
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());

  // print your WiFi shield's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.print(rssi);
  Serial.println(" dBm");
}

// handle interrupt pulses from the taps
void tap1Pulse() {
  tap1->HandlePulse();
}
void tap2Pulse() {
  tap2->HandlePulse();
}

// main loop - once a second, check the accumulated pulse counts and send the START/STOP/CONTINUE/IGNORE message as necessary
// send an ALIVE message every loop to assist debugging
void loop() {
  int cycle = 0;
  while(true) {
    tap1->Loop(cycle);
    tap2->Loop(cycle);

    if(cycle % 10 == 0) {
      Serial.println("ALIVE");
    }
    cycle = (cycle + 1) % 600;
    delay(100);
  }
}


