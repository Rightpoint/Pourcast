#include "Arduino.h"
#include "Reporter.h"
#include "NetworkRequester.h"

class NetworkReporter: public Reporter {
public:
  NetworkReporter(NetworkRequester* requester, const char* tapId);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
private:
  const char* _tapId;
  NetworkRequester* _requester;
};

