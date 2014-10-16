#include <SoftwareSerial.h>
#include <Streaming.h>
#include <PString.h>
#include <WiFlySerial.h>
#include <Time.h>

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

#define REQUEST_BUFFER_SIZE 180
char bufRequest[REQUEST_BUFFER_SIZE];

time_t GetSyncTime() {
  time_t tCurrent = (time_t) wifi.getTime();
  wifi.exitCommandMode();
  return tCurrent;
}

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  Serial.println("setup");

  //wifi.setDebugChannel(&Serial);
  
  pinMode(6, OUTPUT);
  pinMode(7, OUTPUT);
  digitalWrite(6, HIGH);
  digitalWrite(7, HIGH);
//  delay(1000);
  digitalWrite(6, LOW);
  digitalWrite(7, HIGH);
  //delay(1000);
  digitalWrite(6, HIGH);
  digitalWrite(7, LOW);
  //delay(1000);
  digitalWrite(6, HIGH);
  digitalWrite(7, HIGH);
  
  Serial.println("Starting");
  wifi.begin();
  wifi.setPassphrase("");    
  Serial.println("Joining");
  
  digitalWrite(6, LOW);
  digitalWrite(7, HIGH);
  //Serial.println("Joining");
  if (!wifi.join("")) {
     // Handle the failure
    Serial.println("Failed to connect");
    digitalWrite(7, HIGH);
  } else{
    Serial.println("Connected");
    digitalWrite(7, LOW);
  }
  digitalWrite(6, HIGH);

  Serial << "Free: " << freeMemory() << endl;

  wifi.SendCommand("set comm remote 0", ">",bufRequest, REQUEST_BUFFER_SIZE);
  wifi.setNTP("nist1-la.ustiming.org"); 
  wifi.setNTP_Update_Frequency(" 15");
  wifi.getDeviceStatus();
  setTime( wifi.getTime() );
  delay(1000);
  setSyncProvider( GetSyncTime );
  
  
  // Set timezone adjustment: CDT is -5h.  Adjust to your local timezone.
  adjustTime( (long) (-5 * 60 * 60) );
  
  //WiFly.configure(WIFLY_BAUD, 38400);

//  Serial.println(WiFly.ip());

  http = new NetworkRequester(&wifi, "pourcast.labs.rightpoint.com", 80, 9);
  http->LogMessage("Initializing");
  tap1 = new Tap(new MultiReporter(new LEDReporter(4), new LEDReporter(10), new NetworkReporter(http, "535c61a951aa0405287989ec")));
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new LEDReporter(5), new LEDReporter(11), new NetworkReporter(http, "537d28db51aa04289027cde5")));
  attachInterrupt(1, tap2Pulse, RISING);
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
  Serial << "Free: " << freeMemory() << endl;
  http->LogMessage("Starting main loop");
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

