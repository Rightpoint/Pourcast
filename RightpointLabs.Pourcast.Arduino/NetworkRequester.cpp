#include <Dhcp.h>
#include <Dns.h>
#include <Ethernet.h>
#include <EthernetClient.h>

#include "Arduino.h"
#include "NetworkRequester.h"
#include <Streaming.h>
#include <PString.h>
#include "MemoryFree.h"

NetworkRequester::NetworkRequester(const char* host, byte pin, Print* debug) { 
  _host = host;
  _pin = pin;
  _debug = debug;
  _minFreeMeory = 50000;
  pinMode(_pin, OUTPUT);
  digitalWrite(_pin, HIGH);
}

void NetworkRequester::MakeRequest(const char* url){
  digitalWrite(_pin, LOW);
  *_debug << url << endl;
  
  // Build GET expression
  
  char bufRequest[192];
  PString strRequest(bufRequest, 192);

  int newFree = freeMemory();
  if(newFree < _minFreeMemory) {
    _minFreeMemory = newFree;
  }  

  strRequest << F("GET ") << url 
     << F(" HTTP/1.0") << "\n"
     << F("Host: ") << _host << "\n"
     << "\n\n";
  //*_debug << strRequest << endl;
  //*_debug << F("Free: ") << freeMemory() << endl;
  
  EthernetClient client;
  if (client.connect(_host, 80)) {
    *_debug << F("Connected") << endl;
    client << (const char*) strRequest << endl; 
    *_debug << F("Request sent") << endl;
    while (client.connected())
      while(client.available())
        client.read();
//        _debug->write((byte) client.read());
    *_debug << endl << F("response drained") << endl;
    client.stop();
    
    *_debug << F("connection closed") << endl;
  } else {
    _debug->println(F("Connection failed"));
  }

  *_debug << url << F(" COMPLETE") << endl;
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
  //*_debug << F("Dec2Hex ") << buf << F(" + ") << ch << F(" -> ") << outBuf << endl;
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
  
  *_debug << F("Free: ") << freeMemory() << endl;
  MakeRequest(buf);
}
void NetworkRequester::LogMessage(const char* message){
  char buf[128];
  PString pBuf(buf, 128);
  pBuf << F("/api/Status/logMessage?message=");
  EscapeMessage(&pBuf, message);
  
  *_debug << F("Free: ") << freeMemory() << endl;
  MakeRequest(buf);
}
