#include "Arduino.h"
#include "NetworkRequester.h"
#include <Streaming.h>
#include <PString.h>
#include <WiFlySerial.h>

NetworkRequester::NetworkRequester(WiFlySerial* wiFly, const char* host, int port, byte pin) { 
  _wiFly = wiFly;
  _host = host;
  _port = port;
  _pin = pin;
  pinMode(_pin, OUTPUT);
  digitalWrite(_pin, HIGH);
}

void NetworkRequester::MakeRequest(String url){
  digitalWrite(_pin, LOW);
  Serial.println(url);
  // Build GET expression
  
  char bufRequest[512];
  PString strRequest(bufRequest, 512);

  strRequest << F("GET ") << url 
     << F(" HTTP/1.1") << "\n"
     << F("Host: ") << _host << "\n"
     << F("Connection: close") << "\n"
     << "\n\n";

  if (_wiFly->openConnection(_host)) {
    Serial << strRequest << endl;
    *_wiFly << (const char*) strRequest << endl; 
    while (  _wiFly->isConnectionOpen() ) {
      if (  _wiFly->available() > 0 ) {
        Serial << (char) _wiFly->read();
      }
    }
  } else {
    Serial.println("Connection failed");
  }

  Serial.print(url);
  Serial.println(" COMPLETE");
  digitalWrite(_pin, HIGH);
}

void NetworkRequester::Heartbeat(){
  MakeRequest("/api/Status/heartbeat");
}
// Dec2Hex + EscapeMessage based on Toolbox.NETMF.Tools.RawUrlEncode
String Dec2Hex(char ch, int len) {
  char buf[6];
  snprintf(buf, 5, "%2x", ch);
  return buf;
}
String EscapeMessage(String message) {
  String retVal = "";
  for(int i = 0; i < message.length(); i++) {
    char ch = message.charAt(i);
    if (
       ch == 0x2d                  // -
       || ch == 0x5f               // _
       || ch == 0x2e               // .
       || ch == 0x7e               // ~
       || (ch > 0x2f && ch < 0x3a) // 0-9
       || (ch > 0x40 && ch < 0x5b) // A-Z
       || (ch > 0x60 && ch < 0x7b) // a-z
       )
    {
        retVal += ch;
    }
    else
    {
        // Calculates the hex value in some way
        retVal += "%" + Dec2Hex(ch, 2);
    }
  }
  return retVal;
}
void NetworkRequester::LogMessage(String message){
  MakeRequest("/api/Status/logMessage?message=" + EscapeMessage(message));
}
