#include "Tap.h"
#include "TapMonitor.h"

const unsigned long _pulsesToGoFast = 50;
const unsigned long _pulsesToGoSlow = 0;

void requestTempMeasurement(OneWire* oneWire, const byte* sensor) {
  oneWire->reset();
  oneWire->select(sensor);
  oneWire->write(0x44);        // start conversion, no parasite power
}

float readTemp(OneWire* oneWire, const byte* sensor) {
  byte data[12];
  
  oneWire->reset();
  oneWire->select(sensor);    
  oneWire->write(0xBE);         // Read Scratchpad

  for ( int i = 0; i < 9; i++) {           // we need 9 bytes
    data[i] = oneWire->read();
  }
  
  if(OneWire::crc8(data, 8) != data[8]) {
    return -1001; // invalid
  }
  
  // Convert the data to actual temperature
  // because the result is a 16 bit signed integer, it should
  // be stored to an "int16_t" type, which is always 16 bits
  // even when compiled on a 32 bit processor.
  int16_t raw = (data[1] << 8) | data[0];
  byte cfg = (data[4] & 0x60);
  // at lower res, the low bits are undefined, so let's zero them
  if (cfg == 0x00) raw = raw & ~7;  // 9 bit resolution, 93.75 ms
  else if (cfg == 0x20) raw = raw & ~3; // 10 bit res, 187.5 ms
  else if (cfg == 0x40) raw = raw & ~1; // 11 bit res, 375 ms
  //// default is 12 bit resolution, 750 ms conversion time

  float celsius, fahrenheit;
  celsius = (float)raw / 16.0;
  fahrenheit = celsius * 1.8 + 32.0;

  return fahrenheit;
}

Tap::Tap(OneWire* oneWire, byte* tempSensorAddress, const char* kegId, int weightPin) {
  _oneWire = oneWire;
  _tempSensorAddress = tempSensorAddress;
  _pulsesWaiting = 0;
  _lastSent = 0;
  _sendSpeed = 0;
  _kegId = kegId;
  _temperature = 0;
  _weightPin = weightPin;
  requestTempMeasurement(_oneWire, _tempSensorAddress);
}
long Tap::Clear() {
  long pulses = _pulsesWaiting;
  _pulsesWaiting = 0;
  return pulses;
}
void Tap::Loop(int cycle) {
  long pulses = _pulsesWaiting;

  // ask one second before we actually read the temp
  if(cycle % 600 == 590) {
    requestTempMeasurement(_oneWire, _tempSensorAddress);
  }

  // speeds + transitions
  //   0 = send every minute, if we get pulses, goto 3
  //   1 = send every second, if we get no pulses, goto 0
  //   2 = send now, then go to 1
  //   3 = send now, then go to 2
  // this results in us sending the pending pulses followed quickly by enough pulses/sec to start the flow, then holding it open and incrementing

  if(_sendSpeed == 0 && pulses >= _pulsesToGoFast) {
    // speed up
    _sendSpeed = 3;
  }

  if(_lastSent == 0 || _sendSpeed >= 2 || (_sendSpeed == 1 && cycle % 60 == 0) || (cycle % 600 != 0)) {
    if(_sendSpeed == 0) {
      // only bother reading the temperature if we're in the slowest mode
      _temperature = readTemp(_oneWire, _tempSensorAddress);
    }

    // measure and send
    unsigned long thisSend = millis();
    deviceSend(thisSend - _lastSent, _temperature, analogRead(_weightPin), pulses, _kegId);
    _lastSent = thisSend;

    if(pulses <= _pulsesToGoSlow) {
      // slow down
      _sendSpeed = 0;
    }
  }

  if(_sendSpeed >= 2) {
    _sendSpeed--;
  }
}



