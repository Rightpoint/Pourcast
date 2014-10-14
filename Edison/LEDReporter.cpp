#include "LEDReporter.h"

LEDReporter::LEDReporter(byte pin) { 
  _pin = pin;
  pinMode(_pin, OUTPUT);
}

void LEDReporter::ReportStop(long pulses){
  digitalWrite(_pin, LOW);
}
void LEDReporter::ReportContinue(long pulses) {
}
void LEDReporter::ReportStart(long pulses){
  digitalWrite(_pin, HIGH);
}
void LEDReporter::ReportIgnore(long pulses){
}
