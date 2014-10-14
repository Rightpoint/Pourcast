#include "Tap.h"

const long _startThreshold = 30;
const long _continueThreshold = 5;

Tap::Tap(Reporter* reporter) {
  _reporter = reporter;
  _pulses = 0;
  _lastPulses = 0;
}
long Tap::Clear() {
  long pulses = _pulses;
  _pulses = 0;
  return pulses;
}
void Tap::Loop(int cycle) {
  long pulses = _pulses;
  if(_lastPulses != 0 && cycle % 10 == 0) {
    // in progress and on the second
    if(pulses - _lastPulses < _continueThreshold) {
      // complete
      _pulses = 0;
      _lastPulses = 0;
      _reporter->ReportStop(pulses);
    } 
    else {
      // continuing
      _lastPulses = pulses;
      _reporter->ReportContinue(pulses);
    }
  } 
  else if(_lastPulses == 0) {
    // not in progress
    if(pulses >= _startThreshold) {
      // starting
      _lastPulses = pulses;
      _reporter->ReportStart(pulses);
    } 
    else if(pulses > 0 && cycle % 10 == 0) {
      // ignored
      _pulses = 0;
      _reporter->ReportIgnore(pulses);
    }
  }
}

