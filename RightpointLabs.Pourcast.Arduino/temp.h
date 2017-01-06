#include <OneWire.h>

void requestTempMeasurement(OneWire* oneWire, const byte* sensor);
float readTemp(OneWire* oneWire, const byte* sensor);
