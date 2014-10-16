#define DEBUG_LEVEL 5
#include "WiFlyReporter.h"

WiFlyReporter::WiFlyReporter(WiFlyHttp* http, String tapId) { 
  _http = http;
  _tapId = tapId;
}

void WiFlyReporter::MakeRequest(String url){
  _http->MakeRequest(url);
}
void WiFlyReporter::ReportStop(long pulses){
  MakeRequest("/api/Tap/" + _tapId + "/StopPour?volume=" + (pulses / 1000));
}
void WiFlyReporter::ReportContinue(long pulses) {
  MakeRequest("/api/Tap/" + _tapId + "/Pouring?volume=" + (pulses / 1000));
}
void WiFlyReporter::ReportStart(long pulses){
  MakeRequest("/api/Tap/" + _tapId + "/StartPour");
}
void WiFlyReporter::ReportIgnore(long pulses){
}
void WiFlyReporter::Heartbeat(){
  MakeRequest("/api/Status/heartbeat");
}

