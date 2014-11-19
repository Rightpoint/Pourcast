#include "Tap.h"
#include <Streaming.h>
#include <PString.h>
#include <OneWire.h>

const unsigned long _maxPulsesPerMilli = 3;
const long _startThreshold = 30;
const long _continueThreshold = 5;

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

Tap::Tap(Reporter* reporter, OneWire* oneWire, byte* tempSensorAddress) {
  _reporter = reporter;
  _oneWire = oneWire;
  _tempSensorAddress = tempSensorAddress;
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
  if(cycle % 100 == 0) {
    requestTempMeasurement(_oneWire, _tempSensorAddress);
  }
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
  if(cycle % 100 == 9) {
    float value = readTemp(_oneWire, _tempSensorAddress);
    if(value > -1000) {
      _reporter->ReportTemperature(value);
    }
  }
}


