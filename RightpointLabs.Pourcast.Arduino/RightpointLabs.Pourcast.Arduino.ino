volatile long _tap1Pulses = 0;
volatile long _tap2Pulses = 0;
const long _startThreshold = 30;
const long _continueThreshold = 5;

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  attachInterrupt(0, tap1Pulse, RISING);
  attachInterrupt(1, tap2Pulse, RISING);
}

// handle interrupt pulses from the taps
void tap1Pulse() {
  ++_tap1Pulses;
}
void tap2Pulse() {
  ++_tap2Pulses;
}

// main loop - once a second, check the accumulated pulse counts and send the START/STOP/CONTINUE/IGNORE message as necessary
// send an ALIVE message every loop to assist debugging
void loop() {
  long lastTap1Pulses = 0;
  long lastTap2Pulses = 0;
  int cycle = 0;
  while(true) {
    long tap1Pulses = _tap1Pulses;
    if(lastTap1Pulses != 0 && cycle % 10 == 0) {
      // in progress and on the second
      if(tap1Pulses - lastTap1Pulses < _continueThreshold) {
        // complete
        _tap1Pulses = 0;
        lastTap1Pulses = 0;
        Serial.print("STOP 1 ");
        Serial.println(tap1Pulses, DEC);
      } else {
        // continuing
        Serial.print("CONTINUE 1 ");
        Serial.println(tap1Pulses, DEC);
        lastTap1Pulses = tap1Pulses;
      }
    } else if(lastTap1Pulses == 0) {
      // not in progress
      if(tap1Pulses >= _startThreshold) {
        // starting
        Serial.print("START 1 ");
        Serial.println(tap1Pulses, DEC);
        lastTap1Pulses = tap1Pulses;
      } else if(tap1Pulses > 0 && cycle % 10 == 0) {
        // ignored
        _tap1Pulses = 0;
        Serial.print("IGNORE 1 ");
        Serial.println(tap1Pulses, DEC);
      }
    }
    
    long tap2Pulses = _tap2Pulses;
    if(lastTap2Pulses != 0 && cycle % 10 == 0) {
      // in progress and on the second
      if(tap2Pulses - lastTap2Pulses < _continueThreshold) {
        // complete
        _tap2Pulses = 0;
        lastTap2Pulses = 0;
        Serial.print("STOP 2 ");
        Serial.println(tap2Pulses, DEC);
      } else {
        // continuing
        Serial.print("CONTINUE 2 ");
        Serial.println(tap2Pulses, DEC);
        lastTap2Pulses = tap2Pulses;
      }
    } else if(lastTap2Pulses == 0) {
      // not in progress
      if(tap2Pulses >= _startThreshold) {
        // starting
        Serial.print("START 2 ");
        Serial.println(tap2Pulses, DEC);
        lastTap2Pulses = tap2Pulses;
      } else if(tap2Pulses > 0 && cycle % 20 == 0) {
        // ignored
        _tap2Pulses = 0;
        Serial.print("IGNORE 2 ");
        Serial.println(tap2Pulses, DEC);
      }
    }

    if(cycle % 10 == 0) {
      Serial.println("ALIVE");
    }
    cycle = (cycle + 1) % 100;
    delay(100);
  }
}
