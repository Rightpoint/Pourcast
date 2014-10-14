#include "SerialReporter.h"

SerialReporter::SerialReporter(byte number) { 
  _number = number;
}

void SerialReporter::ReportStop(long pulses){
  Serial.print("STOP ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportContinue(long pulses) {
  Serial.print("CONTINUE ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportStart(long pulses){
  Serial.print("START ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportIgnore(long pulses){
  Serial.print("IGNORE ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}


