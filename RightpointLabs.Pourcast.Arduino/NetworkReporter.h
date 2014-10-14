#include "Arduino.h"
#include "Reporter.h"
#include "NetworkRequester.h"

class NetworkReporter: public Reporter {
public:
  NetworkReporter(NetworkRequester* requester, String tapId);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
  void Heartbeat();
private:
  void MakeRequest(String url);
  String _tapId;
  NetworkRequester* _requester;
};

