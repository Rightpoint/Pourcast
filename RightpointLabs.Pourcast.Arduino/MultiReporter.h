#include "Arduino.h"
#include "Reporter.h"

class MultiReporter: public Reporter {
public:
  MultiReporter(Reporter* reporter1, Reporter* reporter2, Reporter* reporter3);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
  virtual void LogMessage(const char* message);
  virtual void LogMessage(const __FlashStringHelper* message);
private:
  Reporter* _reporter1;
  Reporter* _reporter2;
  Reporter* _reporter3;
};

