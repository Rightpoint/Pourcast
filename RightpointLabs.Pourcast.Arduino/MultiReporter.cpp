#include "MultiReporter.h"

MultiReporter::MultiReporter(Reporter* reporter1, Reporter* reporter2, Reporter* reporter3) { 
  _reporter1 = reporter1;
  _reporter2 = reporter2;
  _reporter3 = reporter3;
}

void MultiReporter::ReportStop(long pulses){
  if(_reporter1 != NULL) {
    _reporter1->ReportStop(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportStop(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportStop(pulses);
  }
}
void MultiReporter::ReportContinue(long pulses) {
  if(_reporter1 != NULL) {
    _reporter1->ReportContinue(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportContinue(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportContinue(pulses);
  }
}
void MultiReporter::ReportStart(long pulses){
  if(_reporter1 != NULL) {
    _reporter1->ReportStart(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportStart(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportStart(pulses);
  }
}
void MultiReporter::ReportIgnore(long pulses){
  if(_reporter1 != NULL) {
    _reporter1->ReportIgnore(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportIgnore(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportIgnore(pulses);
  }
}
void MultiReporter::ReportTemperature(float tempF){
  if(_reporter1 != NULL) {
    _reporter1->ReportTemperature(tempF);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportTemperature(tempF);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportTemperature(tempF);
  }
}
void MultiReporter::LogMessage(const __FlashStringHelper* message){
  if(_reporter1 != NULL) {
    _reporter1->LogMessage(message);
  }
  if(_reporter2 != NULL) {
    _reporter2->LogMessage(message);
  }
  if(_reporter3 != NULL) {
    _reporter3->LogMessage(message);
  }
}
void MultiReporter::LogMessage(const char* message){
  if(_reporter1 != NULL) {
    _reporter1->LogMessage(message);
  }
  if(_reporter2 != NULL) {
    _reporter2->LogMessage(message);
  }
  if(_reporter3 != NULL) {
    _reporter3->LogMessage(message);
  }
}
