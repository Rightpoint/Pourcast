#include "Arduino.h"
#include "Reporter.h"
#include <OneWire.h>

class Tap {
public: 
  Tap(Reporter* reporter, OneWire* oneWire, byte* tempSensorAddress);
  inline void HandlePulse();
  long Clear();
  void Loop(int cycleNumber);
private:
  Reporter* _reporter;
  OneWire* _oneWire;
  byte* _tempSensorAddress;
  volatile long _pulses;
  long _lastPulses;
  unsigned long _pulsesStart;
};
// declared here because we want it to be eligible to be inlined
void Tap::HandlePulse() {
  _pulses++;
}
