#include <OneWire.h>

void requestTempMeasurement(OneWire* oneWire, const byte* sensor) {
  if(sensor == NULL)
    return;
  oneWire->reset();
  oneWire->select(sensor);
  oneWire->write(0x44);        // start conversion, no parasite power
}

float readTemp(OneWire* oneWire, const byte* sensor) {
  if(sensor == NULL)
    return 0;
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
