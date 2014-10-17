#include <SoftwareSerial.h>
#include <Streaming.h>
#include <PString.h>
#include <Time.h>

#include "WiFlySerial.h"
#include "MemoryFree.h"
#include "Tap.h"
#include "MultiReporter.h"
#include "LEDReporter.h"
#include "SerialReporter.h"
#include "NetworkReporter.h"

WiFlySerial wifi(4,5);
Tap* tap1;
Tap* tap2;
NetworkRequester* http;

time_t GetSyncTime() {
  time_t tCurrent = (time_t) wifi.getTime();
  wifi.exitCommandMode();
  return tCurrent;
}

void setNoCommRemote() {
  char bufRequest[60];
  wifi.SendCommand("set comm remote 0", ">",bufRequest, 60);
}

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  Serial.println("setup");
  
  Serial << F("Free: ") << freeMemory() << endl;

  char buf[32];
  PString pBuf(buf, 32);
  
  #include "Credentials.h"

  //wifi.setDebugChannel(&Serial);
  Serial.println(F("Starting"));
  wifi.begin();
  pBuf.begin();
  pBuf << wifiPassword;
  wifi.setPassphrase(pBuf);    
  
  wifi.getDeviceStatus();
  while (! wifi.isifUp() ) {
    Serial.println(F("Joining"));
    pBuf.begin();
    pBuf << wifiSSID;
    if (!wifi.join((char*)(const char*)pBuf)) {
      Serial.println(F("Failed to connect"));
    } else{
      Serial.println(F("Connected"));
      break;
    }
    
    // TODO: use an actual connection to confirm we got a good connection - maybe push this logic down to the network layer?
    
    wifi.getDeviceStatus();
  }
    
  Serial << F("Free: ") << freeMemory() << endl;
  setNoCommRemote();
  Serial << F("Free: ") << freeMemory() << endl;

  Serial << F("Enabling NTP") << endl;
  Serial << F("Free: ") << freeMemory() << endl;
  pBuf.begin();
  pBuf << F("nist1-la.ustiming.org");
  wifi.setNTP(pBuf); 
  pBuf.begin();
  pBuf << F(" 15");
  wifi.setNTP_Update_Frequency(pBuf);
  Serial << F("Getting status") << endl;
  Serial << F("Free: ") << freeMemory() << endl;
  wifi.getDeviceStatus();
  Serial << F("Configuring time") << endl;
  Serial << F("Free: ") << freeMemory() << endl;
  setTime( wifi.getTime() );
  delay(1000);
  setSyncProvider( GetSyncTime );
  
  
  // Set timezone adjustment: CDT is -5h.  Adjust to your local timezone.
  adjustTime( (long) (-5 * 60 * 60) );
  
  //WiFly.configure(WIFLY_BAUD, 38400);

//  Serial.println(WiFly.ip());

  Serial << F("Starting network stuff") << endl;
  Serial << F("Free: ") << freeMemory() << endl;
  http = new NetworkRequester(&wifi, "pourcast.labs.rightpoint.com", 9);
  http->LogMessage(F("Initializing"));
  tap1 = new Tap(new MultiReporter(new LEDReporter(6), new LEDReporter(10), new NetworkReporter(http, "535c61a951aa0405287989ec")));
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new LEDReporter(7), new LEDReporter(11), new NetworkReporter(http, "537d28db51aa04289027cde5")));
  attachInterrupt(1, tap2Pulse, RISING);
  http->LogMessage(F("Done Initializing"));
  Serial << F("Setup complete") << endl;
  Serial << F("Free: ") << freeMemory() << endl;
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
  Serial << F("Free: ") << freeMemory() << endl;
  http->LogMessage(F("Starting Main Loop"));
  return;
  int cycle = 0;
  while(true) {
    tap1->Loop(cycle);
    tap2->Loop(cycle);

    if(cycle % 10 == 0) {
      //Serial.println("ALIVE");
    }
    cycle = (cycle + 1) % 600;
    delay(100);
  }
}

