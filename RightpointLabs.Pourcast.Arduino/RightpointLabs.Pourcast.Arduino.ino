#define DEBUG_LEVEL 5

#include <SPI.h>
#include <Configuration.h>
#include <Debug.h>
#include <ParsedStream.h>
#include <SpiUart.h>
#include <WiFly.h>
#include <WiFlyClient.h>
#include <WiFlyDevice.h>
#include <WiFlyServer.h>
#include <_Spi.h>

#include "Tap.h"
#include "MultiReporter.h"
#include "LEDReporter.h"
#include "SerialReporter.h"
#include "WiFlyReporter.h"

Tap* tap1;
Tap* tap2;
WiFlyHttp* http;

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  
  Serial.println("Starting");
  WiFly.begin();
  Serial.println("Joining");
  if (!WiFly.join("Rightpoint-Guest", "PW")) {
     // Handle the failure
     Serial.println("Failed to connect");
  }
  else{
     Serial.println("Connected");
  }
  
  WiFly.configure(WIFLY_BAUD, 38400);

  Serial.println(WiFly.ip());

  http = new WiFlyHttp("pourcast.labs.rightpoint.com", 80, 9);
  tap1 = new Tap(new MultiReporter(new SerialReporter(1), new LEDReporter(10), new WiFlyReporter(http, "535c61a951aa0405287989ec")));
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new SerialReporter(2), new LEDReporter(11), new WiFlyReporter(http, "537d28db51aa04289027cde5")));
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

