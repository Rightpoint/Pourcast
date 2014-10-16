#define DEBUG_LEVEL 5
#include "NetworkReporter.h"
#include <Streaming.h>
#include <PString.h>

const double PULSES_PER_OZ = 5600.0 / 33.814;

NetworkReporter::NetworkReporter(NetworkRequester* requester, const char* tapId) { 
  _requester = requester;
  _tapId = tapId;
}

String toString(double arg) {
  char buf[128];
  snprintf(buf, 128, "%f", arg);
  return buf;
}

void NetworkReporter::ReportStop(long pulses){
  double oz = pulses / PULSES_PER_OZ;
  
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << "/api/Tap/" << _tapId << "/StopPour?volume=" << oz;
  
  _requester->MakeRequest(buf);
}
void NetworkReporter::ReportContinue(long pulses) {
  double oz = pulses / PULSES_PER_OZ;
  
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << "/api/Tap/" << _tapId << "/Pouring?volume=" << oz;
  
  _requester->MakeRequest(buf);
}
void NetworkReporter::ReportStart(long pulses){
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << "/api/Tap/" << _tapId << "/StartPour";
  
  _requester->MakeRequest(buf);
}
void NetworkReporter::ReportIgnore(long pulses){
}
