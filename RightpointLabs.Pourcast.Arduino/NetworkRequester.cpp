#include "Arduino.h"
#include "NetworkRequester.h"
#include <Streaming.h>
#include <PString.h>
#include <WiFlySerial.h>
#include "MemoryFree.h"

NetworkRequester::NetworkRequester(WiFlySerial* wiFly, const char* host, byte pin) { 
  _ip[0] = '\0';
  //strcpy(_ip, "64.68.29.134");
  //strcpy(_ip, "192.168.25.1");
  _wiFly = wiFly;
  _host = host;
  _pin = pin;
  pinMode(_pin, OUTPUT);
  
  ResolveIP();
  
  digitalWrite(_pin, HIGH);
}

bool NetworkRequester::ResolveIP() {
  if(_ip[0] != '\0')
    return true;
    
  // whoa... format is name=IP - bleh, don't want to parse all that...
    
  char readBuffer[48];
  
  char buf[48];
  PString pBuf(buf, 48, "lookup " );
  pBuf << _host;
  
  if(!_wiFly->SendCommand(buf, ">", readBuffer, 48))
    readBuffer[0] = '\0';
  Serial << F("Raw lookup response: ") << readBuffer << endl;
  
  bool isValid = true;
  byte len = strlen(readBuffer);
  byte dots = 0;
  bool wasLastDot = true;
  for(byte i=0; i<len; i++) {
    if(readBuffer[i] == '.') {
      if(wasLastDot) {
        //Serial << F("Extra dot @ ") << i << endl;
        isValid = false;
        break;
      } else{
        //Serial << F("Valid dot @ ") << i << endl;
        dots++;
        wasLastDot = true;
      }
    } else if(readBuffer[i] >= '0' && readBuffer[i] <= '9') {
      wasLastDot = false;
      //Serial << F("Valid digit @ ") << i << endl;
    } else {
      // terminate here
      readBuffer[i] = '\0';
      break;
    }
  }
  isValid &= dots == 3;
    
  if(isValid) {
    strncpy(_ip, readBuffer, 32);
    Serial << F("Resolved ") << _host << " to " << _ip << endl;
  } else{
    Serial << F("Ignored resolution of  ") << _host << " to " << readBuffer  << endl;
    Serial << F("Command was: ") << buf << endl;
    _ip[0] = '\0';
  }
    
  return _ip[0] != '\0';
}

void NetworkRequester::MakeRequest(const char* url){
  digitalWrite(_pin, LOW);
  Serial.println(url);
  
  
  while(!ResolveIP()){
    delay(100);
  }
  
  // Build GET expression
  
  char bufRequest[192];
  PString strRequest(bufRequest, 192);

  strRequest << F("GET ") << url 
     << F(" HTTP/1.0") << "\n"
     << F("Host: ") << _host << "\n"
     << "\n\n";
  //Serial << strRequest << endl;
  //Serial << F("Free: ") << freeMemory() << endl;

  if (_wiFly->openConnection(_ip)) {
    Serial << F("Connected") << endl;
    *_wiFly << (const char*) strRequest << endl; 
    Serial << F("Request sent") << endl;
    while (_wiFly->isConnectionOpen())
      _wiFly->drain();
    Serial << F("response drained") << endl;
    /*
    while (  _wiFly->isConnectionOpen() ) {
      if (  _wiFly->available() > 0 ) {
        Serial << (char) _wiFly->read();
      }
    }
    */
    _wiFly->closeConnection();
    Serial << F("connection closed") << endl;
  } else {
    Serial.println(F("Connection failed"));
  }

  Serial.print(url);
  Serial.println(F(" COMPLETE"));
  digitalWrite(_pin, HIGH);
}

void NetworkRequester::Heartbeat(){
  char buf[32];
  PString(buf, 32, F("/api/Status/heartbeat"));
  MakeRequest(buf);
}
// Dec2Hex + EscapeMessage based on Toolbox.NETMF.Tools.RawUrlEncode
void Dec2Hex(PString* output, char ch, int len) {
  char buf[4];
  PString pBuf(buf, 4, F("%2x"));
  char outBuf[6];
  snprintf(outBuf, 6, buf, ch);
  //Serial << F("Dec2Hex ") << buf << F(" + ") << ch << F(" -> ") << outBuf << endl;
  *output << outBuf;
}
void EscapeMessage(PString* output, const char* message) {
  int len = strlen(message);
  for(int i = 0; i < len; i++) {
    char ch = message[i];
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
      *output << ch;
    }
    else
    {
        // Calculates the hex value in some way
      *output << F("%");
      Dec2Hex(output, ch, 2);
    }
  }
}
void NetworkRequester::LogMessage(const __FlashStringHelper* message){
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << F("/api/Status/logMessage?message=");
  
  char *messageBuf = new char[128];
  PString pMessageBuf(messageBuf, 128, message);
  EscapeMessage(&pBuf, messageBuf);
  delete messageBuf;
  
  Serial << F("Free: ") << freeMemory() << endl;
  MakeRequest(buf);
}
void NetworkRequester::LogMessage(const char* message){
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << F("/api/Status/logMessage?message=");
  EscapeMessage(&pBuf, message);
  
  Serial << F("Free: ") << freeMemory() << endl;
  MakeRequest(buf);
}
