#include "Arduino.h"
#include "Reporter.h"

class SerialReporter: public Reporter {
public:
  SerialReporter(byte number);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
private:
  byte _number;
};

