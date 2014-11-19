#include "Arduino.h"
#include "Reporter.h"

class LEDReporter: public Reporter {
public:
  LEDReporter(byte pin);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
  virtual void ReportTemperature(float tempF);
  virtual void LogMessage(const char* message);
  virtual void LogMessage(const __FlashStringHelper* message);
private:
  byte _pin;
};

