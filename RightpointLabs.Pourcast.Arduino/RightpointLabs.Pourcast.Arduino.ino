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


#include "ApplicationMonitor.h"
#include "MemoryFree.h"
#include "Tap.h"
#include "MultiReporter.h"
#include "LEDReporter.h"
#include "SerialReporter.h"
#include "NetworkReporter.h"
#include <OneWire.h>

class NullPrint : public Print {
  public:
    virtual size_t write(uint8_t);
};
size_t NullPrint::write(uint8_t ch){}

class NetworkPrint : public Print {
  public:
    NetworkPrint (NetworkRequester* http);
    virtual size_t write(uint8_t);
  private:
    NetworkRequester* _http;
    char _buf[128];
    PString _pBuf;
};
NetworkPrint::NetworkPrint(NetworkRequester* http): _pBuf(_buf, 128) {
  _http = http;
}

size_t NetworkPrint::write(uint8_t ch){
  _pBuf << (char)ch;
  if(ch == '\n'){
    _http->LogMessage(_buf);
  }
  _pBuf.begin();
}

Watchdog::CApplicationMonitor ApplicationMonitor;
Tap* tap1;
Tap* tap2;
NetworkRequester* http;
OneWire oneWire(6);

byte sensor1[] = { 0x28, 0x8A, 0x8E, 0x2A, 0x06, 0x00, 0x00, 0xE8 };
byte sensor2[] = { 0x28, 0xA9, 0xD2, 0xFA, 0x05, 0x00, 0x00, 0x56 };

// Enter a MAC address for your controller below.
// Newer Ethernet shields have a MAC address printed on a sticker on the shield
byte mac[] = { 0x90, 0xA2, 0xDA, 0x0F, 0x84, 0xC2 };

void dumpWatchdogHistory() {
  NetworkPrint np(http);
  ApplicationMonitor.Dump(np);
  ApplicationMonitor.Dump(Serial);
}

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  ApplicationMonitor.DisableWatchdog();
  Serial.begin(9600);
  while(!Serial);
  Serial.println("setup");
  
  // let's light up briefly while waiting for the initial temp reading
  pinMode(9, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(7, OUTPUT);
  digitalWrite(9, HIGH);
  digitalWrite(8, HIGH);
  digitalWrite(7, HIGH);
  delay(1000);
  digitalWrite(9, HIGH);
  digitalWrite(8, LOW);
  digitalWrite(7, LOW);
  
  Serial << F("Free: ") << freeMemory() << endl;

  char buf[32];
  PString pBuf(buf, 32);
  
  Serial.println(F("Starting"));
  Ethernet.begin(mac);
  
  Serial << F("Starting network stuff") << endl;
  Serial << F("Free: ") << freeMemory() << endl;
  http = new NetworkRequester("pourcast.labs.rightpoint.com", 9, new NullPrint());
  http->LogMessage(F("Initializing"));
  
  dumpWatchdogHistory();
  
  NetworkReporter* tap1Reporter = new NetworkReporter(http, "535c61a951aa0405287989ec"); 
  NetworkReporter* tap2Reporter = new NetworkReporter(http, "537d28db51aa04289027cde5"); 
  
  tap1 = new Tap(new MultiReporter(new SerialReporter(1), new LEDReporter(8), tap1Reporter), &oneWire, sensor1);
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new SerialReporter(2), new LEDReporter(7), tap2Reporter), &oneWire, sensor2);
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

  // and now we blink a few times to confirm we're starting up, and to wait for any phantom pours to clear
  for(int i=0; i<10; i++) {
    digitalWrite(9, LOW);
    digitalWrite(8, LOW);
    digitalWrite(7, LOW);
    delay(250);
    digitalWrite(9, HIGH);
    digitalWrite(8, HIGH);
    digitalWrite(7, HIGH);
    delay(250);
  }

  // and now go back to normal states
  digitalWrite(9, HIGH);
  digitalWrite(8, LOW);
  digitalWrite(7, LOW);
   
  long tap1Pulses = tap1->Clear();
  long tap2Pulses = tap2->Clear();
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << "Startup delay complete (ignored " << tap1Pulses << " and " << tap2Pulses << ")";
  http->LogMessage(buf);


  pBuf.begin();
  pBuf << F("Light show complete, really starting");
  http->LogMessage(buf);
}

unsigned long lastWatchTime = 0;
unsigned long lastWatch = 0;
unsigned long maxWatch = 0;
    
void writeStatus() {
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << "Status: free memory: " << freeMemory() << ", Lowest: " << http->_minFreeMemory << ", lastWatch: " << lastWatch << ", maxWatch: " << maxWatch;
  http->LogMessage(buf);
}

void resetWatchdog() {
  ApplicationMonitor.IAmAlive();
  unsigned long thisWatchTime = micros();
  if(thisWatchTime > lastWatchTime) {
    lastWatch = thisWatchTime - lastWatchTime;
  } else {
    lastWatch = 4294967295 - lastWatchTime + thisWatchTime;
  }
  lastWatchTime = thisWatchTime;
  if(lastWatch > maxWatch) {
    maxWatch = lastWatch;
  }
}

// main loop - once a second, check the accumulated pulse counts and send the START/STOP/CONTINUE/IGNORE message as necessary
// send an ALIVE message every loop to assist debugging
void loop() {
  startupDelay();
  
  ApplicationMonitor.EnableWatchdog(Watchdog::CApplicationMonitor::Timeout_8s);
  lastWatchTime = micros();
  
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
    resetWatchdog();
    delay(100);
  }
}

