#include "Arduino.h"

class Tap {
public: 
  Tap(byte kegNum);
  Tap(byte kegNum, byte weightPin);
  inline void HandlePulse();
  long Clear();
  void Loop(int cycleNumber);
private:
  volatile long _pulsesWaiting;
  long _lastSent;
  byte _sendSpeed;
  byte _kegNum;
  short _weightPin;
};
// declared here because we want it to be eligible to be inlined
void Tap::HandlePulse() {
  _pulsesWaiting++;
}

