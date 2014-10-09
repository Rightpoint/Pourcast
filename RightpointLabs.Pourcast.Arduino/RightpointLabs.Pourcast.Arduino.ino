volatile long _tap1Pulses = 0;
volatile long _tap2Pulses = 0;
const long _startThreshold = 50;

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  attachInterrupt(0, tap1Pulse, RISING);
  attachInterrupt(1, tap2Pulse, RISING);
}

// handle interrupt pulses from the taps and send the START message
void tap1Pulse() {
  long pulses = ++_tap1Pulses;
  if(pulses == _startThreshold) {
    Serial.print("START 1 ");
    Serial.println(pulses, DEC);
  }
}
void tap2Pulse() {
  long pulses = ++_tap2Pulses;
  if(pulses == _startThreshold) {
    Serial.print("START 2 ");
    Serial.println(pulses, DEC);
  }
}

// main loop - once a second, check the accumulated pulse counts and send the STOP/CONTINUE/IGNORE message as necessary
// send an ALIVE message every loop to assist debugging
void loop() {
  long lastTap1Pulses = 0;
  long lastTap2Pulses = 0;
  while(true) {
    long tap1Pulses = _tap1Pulses;
    if(lastTap1Pulses != 0 && lastTap1Pulses == tap1Pulses) {
      // complete
      _tap1Pulses = 0;
      lastTap1Pulses = 0;
      Serial.print("STOP 1 ");
      Serial.println(tap1Pulses, DEC);
    } 
    else if(lastTap1Pulses != tap1Pulses) {
      if(tap1Pulses >= _startThreshold) {
        // in progress
        Serial.print("CONTINUE 1 ");
        Serial.println(tap1Pulses, DEC);
        lastTap1Pulses = tap1Pulses;
      }
      else {
        // ignored
        _tap1Pulses = 0;
        Serial.print("IGNORE 1 ");
        Serial.println(tap1Pulses, DEC);
      }
    }

    long tap2Pulses = _tap2Pulses;
    if(lastTap2Pulses != 0 && lastTap2Pulses == tap2Pulses) {
      // complete
      _tap2Pulses = 0;
      lastTap2Pulses = 0;
      Serial.print("STOP 2 ");
      Serial.println(tap2Pulses, DEC);
    } 
    else if(lastTap2Pulses != tap2Pulses) {
      if(tap2Pulses >= _startThreshold) {
        // in progress
        Serial.print("CONTINUE 2 ");
        Serial.println(tap2Pulses, DEC);
        lastTap2Pulses = tap2Pulses;
      }
      else {
        // ignored
        _tap2Pulses = 0;
        Serial.print("IGNORE 2 ");
        Serial.println(tap2Pulses, DEC);
      }
    }

    Serial.println("ALIVE");
    delay(1000);
  }
}
