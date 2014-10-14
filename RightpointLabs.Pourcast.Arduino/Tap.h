#include "Arduino.h"
#include "Reporter.h"

class Tap {
public: 
  Tap(Reporter* reporter);
  inline void HandlePulse();
  void Loop(int cycleNumber);
private:
  Reporter* _reporter;
  volatile long _pulses;
  long _lastPulses;
};
// declared here because we want it to be eligible to be inlined
void Tap::HandlePulse() {
  _pulses++;
}
