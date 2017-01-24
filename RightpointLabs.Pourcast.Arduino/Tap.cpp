#include "Log.h"
#include "Tap.h"
#include "TapMonitor.h"
#include "config.h"

const unsigned long _pulsesToGoFast = 50;
const unsigned long _pulsesToGoSlow = 0;

Tap::Tap(byte kegNum) {
  _kegNum = kegNum;
  _weight = NULL;
  _pulsesWaiting = 0;
  _lastSent = 0;
  _sendSpeed = 0;
}
Tap::Tap(byte kegNum, Q2HX711 *weight) {
  _kegNum = kegNum;
  _weight = weight;
  _pulsesWaiting = 0;
  _lastSent = 0;
  _sendSpeed = 0;
}
long Tap::Clear() {
  long pulses = _pulsesWaiting;
  _pulsesWaiting = 0;
  return pulses;
}
void Tap::Loop(int cycle) {
  long pulses = _pulsesWaiting;

  // speeds + transitions
  //   0 = send every minute, if we get pulses, goto 3
  //   1 = send every second, if we get no pulses, goto 0
  //   2 = send now, then go to 1
  //   3 = send now, then go to 2
  // this results in us sending the pending pulses followed quickly by enough pulses/sec to start the flow, then holding it open and incrementing

  if(_sendSpeed == 0 && pulses >= _pulsesToGoFast) {
    // speed up
    Log("Speeding up to 3");
    _sendSpeed = 3;
    
  }

  if(_lastSent == 0 || _sendSpeed >= 2 || (_sendSpeed == 1 && cycle % SEND_TAP_UPDATE_WHILE_POURING_EVERY == 0) || (cycle % SEND_TAP_UPDATE_EVERY == 0)) {
    // measure and send
    unsigned long thisSend = millis();
    deviceSend(thisSend - _lastSent, _weight == NULL ? -1 : _weight->read(), pulses, _kegNum, _sendSpeed);
    _lastSent = thisSend;

    // mark off how many we sent, may lose occasional pulses here...
    _pulsesWaiting -= pulses;

    if(pulses <= _pulsesToGoSlow) {
      // slow down
      Log("Slowing down to 0");
      _sendSpeed = 0;
    }
  }

  if(_sendSpeed >= 2) {
    _sendSpeed--;
    Log("Slowing down to");
    Log(_sendSpeed);
  }
}



