#include "Arduino.h"

#ifndef REPORTER_H
#define REPORTER_H

class Reporter {
public: 
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
  virtual void ReportTemperature(float tempF);
  virtual void LogMessage(const char* message);
  virtual void LogMessage(const __FlashStringHelper* message);
};

#endif
