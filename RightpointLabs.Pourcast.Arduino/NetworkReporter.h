#include "Arduino.h"
#include "Reporter.h"
#include "WiFlyHttp.h"

class WiFlyReporter: public Reporter {
public:
  WiFlyReporter(WiFlyHttp* http, String tapId);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
  void Heartbeat();
private:
  void MakeRequest(String url);
  String _tapId;
  WiFlyHttp* _http;
};

