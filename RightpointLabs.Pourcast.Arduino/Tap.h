#include "Arduino.h"
#include <Q2HX711.h>

class Tap {
public: 
  Tap(byte kegNum);
  Tap(byte kegNum, Q2HX711 *weight);
  inline void HandlePulse();
  long Clear();
  void Loop(int cycleNumber);
private:
  volatile long _pulsesWaiting;
  long _lastSent;
  byte _sendSpeed;
  byte _kegNum;
  Q2HX711 *_weight;
};
// declared here because we want it to be eligible to be inlined
void Tap::HandlePulse() {
  _pulsesWaiting++;
}

