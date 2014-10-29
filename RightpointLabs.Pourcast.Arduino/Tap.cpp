#include "Tap.h"
#include <Streaming.h>
#include <PString.h>

const unsigned long _maxPulsesPerMilli = 3;
const long _startThreshold = 30;
const long _continueThreshold = 5;

Tap::Tap(Reporter* reporter) {
  _reporter = reporter;
  _pulses = 0;
  _lastPulses = 0;
  _pulsesStart = 0;
}
long Tap::Clear() {
  long pulses = _pulses;
  _pulses = 0;
  return pulses;
}
void ReportFake(Reporter* reporter, unsigned long duration, unsigned long pulses, const __FlashStringHelper* messageType) {
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << F("Detected fake pour of ") << pulses << F(" for ") << duration << F(" - skipping ") << messageType << F(" event");
  reporter->LogMessage(buf);
}
void Tap::Loop(int cycle) {
  long pulses = _pulses;
  if(_lastPulses != 0 && cycle % 10 == 0) {
    // in progress and on the second
    unsigned long duration = millis() - _pulsesStart;
    if(pulses - _lastPulses < _continueThreshold) {
      // complete
      _pulses = 0;
      _lastPulses = 0;
      if(pulses  > duration * _maxPulsesPerMilli) {
        ReportFake(_reporter, duration, pulses, F("stop"));
        pulses = 1;
      }
      _reporter->ReportStop(pulses);
    } 
    else {
      // continuing
      _lastPulses = pulses;
      if(pulses  > duration * _maxPulsesPerMilli) {
        ReportFake(_reporter, duration, pulses, F("continue"));
      } else {
        _reporter->ReportContinue(pulses);
      }
    }
  } 
  else if(_lastPulses == 0) {
    // not in progress
    if(pulses >= _startThreshold) {
      // starting
      _pulsesStart = millis();
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

