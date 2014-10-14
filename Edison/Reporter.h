#include "Arduino.h"

#ifndef REPORTER_H
#define REPORTER_H

class Reporter {
public: 
  virtual void ReportStop(long pulses) {}
  virtual void ReportContinue(long pulses) {}
  virtual void ReportStart(long pulses) {}
  virtual void ReportIgnore(long pulses) {}
};

#endif
