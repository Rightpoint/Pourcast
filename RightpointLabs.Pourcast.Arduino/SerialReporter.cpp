#include "SerialReporter.h"
#include <Streaming.h>

SerialReporter::SerialReporter(byte number) { 
  _number = number;
}

void SerialReporter::ReportStop(long pulses){
  Serial.print(F("STOP ")  );
  Serial.print(_number, DEC);
  Serial.print(F(" "));
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportContinue(long pulses) {
  Serial.print(F("CONTINUE "));
  Serial.print(_number, DEC);
  Serial.print(F(" "));
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportStart(long pulses){
  Serial.print(F("START "));
  Serial.print(_number, DEC);
  Serial.print(F(" "));
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportIgnore(long pulses){
  Serial.print(F("IGNORE "));
  Serial.print(_number, DEC);
  Serial.print(F(" "));
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportTemperature(float tempF) {
  Serial.print(F("TEMPERATURE "));
  Serial.print(_number, DEC);
  Serial.print(F(" "));
  Serial.println(tempF);
}
void SerialReporter::LogMessage(const __FlashStringHelper* message){
  Serial << message << endl;
}
void SerialReporter::LogMessage(const char* message){
  Serial << message << endl;
}
