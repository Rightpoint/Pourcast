#include "Arduino.h"
#include <OneWire.h>

class Tap {
public: 
  Tap(OneWire* oneWire, byte* tempSensorAddress, const char* kegId, int weightPin);
  inline void HandlePulse();
  long Clear();
  void Loop(int cycleNumber);
private:
  OneWire* _oneWire;
  byte* _tempSensorAddress;
  volatile long _pulsesWaiting;
  long _lastSent;
  int _sendSpeed;
  const char* _kegId;
  int _temperature;
  int _weightPin;
};
// declared here because we want it to be eligible to be inlined
void Tap::HandlePulse() {
  _pulsesWaiting++;
}

