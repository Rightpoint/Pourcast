#define DEBUG_LEVEL 5
#include "NetworkReporter.h"

const double PULSES_PER_OZ = 5600.0 / 33.814;

NetworkReporter::NetworkReporter(NetworkRequester* requester, String tapId) { 
  _requester = requester;
  _tapId = tapId;
}

String toString(double arg) {
  char buf[128];
  snprintf(buf, 128, "%f", arg);
  return buf;
}

void NetworkReporter::MakeRequest(String url){
  _requester->MakeRequest(url);
}
void NetworkReporter::ReportStop(long pulses){
  double oz = pulses / PULSES_PER_OZ;
  MakeRequest("/api/Tap/" + _tapId + "/StopPour?volume=" + toString(oz));
}
void NetworkReporter::ReportContinue(long pulses) {
  double oz = pulses / PULSES_PER_OZ;
  MakeRequest("/api/Tap/" + _tapId + "/Pouring?volume=" + toString(oz));
}
void NetworkReporter::ReportStart(long pulses){
  MakeRequest("/api/Tap/" + _tapId + "/StartPour");
}
void NetworkReporter::ReportIgnore(long pulses){
}
