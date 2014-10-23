#include <SPI.h>
#include <Dhcp.h>
#include <Dns.h>
#include <Ethernet.h>
#include <EthernetClient.h>
#include <EthernetServer.h>
#include <EthernetUdp.h>
#include <util.h>

#include <SoftwareSerial.h>
#include <Streaming.h>
#include <PString.h>
#include <Time.h>



#include "MemoryFree.h"
#include "Tap.h"
#include "MultiReporter.h"
#include "LEDReporter.h"
#include "SerialReporter.h"
#include "NetworkReporter.h"

Tap* tap1;
Tap* tap2;
NetworkRequester* http;

// Enter a MAC address for your controller below.
// Newer Ethernet shields have a MAC address printed on a sticker on the shield
byte mac[] = { 0x90, 0xA2, 0xDA, 0x0F, 0x84, 0xC2 };

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  Serial.println("setup");
  
  Serial << F("Free: ") << freeMemory() << endl;

  char buf[32];
  PString pBuf(buf, 32);
  
  Serial.println(F("Starting"));
  Ethernet.begin(mac);
  
  Serial << F("Starting network stuff") << endl;
  Serial << F("Free: ") << freeMemory() << endl;
  http = new NetworkRequester("pourcast.labs.rightpoint.com", 9);
  http->LogMessage(F("Initializing"));
  
  NetworkReporter* tap1Reporter = new NetworkReporter(http, "535c61a951aa0405287989ec"); 
  NetworkReporter* tap2Reporter = new NetworkReporter(http, "537d28db51aa04289027cde5"); 
  
  tap1 = new Tap(new MultiReporter(new SerialReporter(1), new LEDReporter(8), tap1Reporter));
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new SerialReporter(2), new LEDReporter(7), tap2Reporter));
  attachInterrupt(1, tap2Pulse, RISING);
  http->LogMessage(F("Clearing any active pour"));
  
  tap1Reporter->ReportStop(0);
  tap2Reporter->ReportStop(0);
  
  Serial << F("Setup complete") << endl;
  http->LogMessage(F("Done Initializing"));
  Serial << F("Free: ") << freeMemory() << endl;
}

// handle interrupt pulses from the taps
void tap1Pulse() {
  tap1->HandlePulse();
}
void tap2Pulse() {
  tap2->HandlePulse();
}

void startupDelay() {
  http->LogMessage("Begining startup delay");
  delay(2000);
  long tap1Pulses = tap1->Clear();
  long tap2Pulses = tap2->Clear();
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << "Startup delay complete (ignored " << tap1Pulses << " and " << tap2Pulses << ")";
  http->LogMessage(buf);
}

void writeStatus() {
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << "Status: free memory: " << freeMemory();
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

    if(cycle % 600 == 0) {
      writeStatus();
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

