#define DEBUG_LEVEL 5
#include "NetworkReporter.h"

  NetworkReporter::NetworkReporter(NetworkRequester* requester, String tapId) { 
  _requester = requester;
  _tapId = tapId;
}

void NetworkReporter::MakeRequest(String url){
  _requester->MakeRequest(url);
}
void NetworkReporter::ReportStop(long pulses){
  MakeRequest("/api/Tap/" + _tapId + "/StopPour?volume=" + (pulses / 1000));
}
void NetworkReporter::ReportContinue(long pulses) {
  MakeRequest("/api/Tap/" + _tapId + "/Pouring?volume=" + (pulses / 1000));
}
void NetworkReporter::ReportStart(long pulses){
  MakeRequest("/api/Tap/" + _tapId + "/StartPour");
}
void NetworkReporter::ReportIgnore(long pulses){
}
void NetworkReporter::Heartbeat(){
  MakeRequest("/api/Status/heartbeat");
}

