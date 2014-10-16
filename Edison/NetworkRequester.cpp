#include "Arduino.h"
#include "NetworkRequester.h"
#include <WiFi.h>
#include <WiFiClient.h>
#include <Streaming.h>
#include <PString.h>

NetworkRequester::NetworkRequester(const char* host, int port, byte pin) { 
  _host = host;
  _port = port;
  _pin = pin;
  pinMode(_pin, OUTPUT);
  digitalWrite(_pin, HIGH);
}

void NetworkRequester::MakeRequest(const char* url){
  digitalWrite(_pin, LOW);
  Serial.println(url);
  WiFiClient client;
  if (client.connect(_host, _port)) {
    client 
        << "GET " << url << " HTTP/1.0" << endl
        << "HOST: " << _host << endl 
        << endl;

    #if false
    byte buffer[128];
    int read = 0;
    while(client.connected() && (read = client.read(buffer, 128)) > 0) {
      Serial.write(buffer, read);
    #else
    while(client.connected() && client.read() != -1) {
    #endif
    }
  }
  digitalWrite(_pin, HIGH);
}

void NetworkRequester::Heartbeat(){
  MakeRequest("/api/Status/heartbeat");
}
// Dec2Hex + EscapeMessage based on Toolbox.NETMF.Tools.RawUrlEncode
void Dec2Hex(PString* output, char ch, int len) {
  char buf[6];
  snprintf(buf, 5, "%2x", ch);
  *output << buf;
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
        *output << "%";
        Dec2Hex(output, ch, 2);
    }
  }
}

void NetworkRequester::LogMessage(const char* message){
  char buf[256];
  PString pBuf(buf, 256, "/api/Status/logMessage?message=");
  EscapeMessage(&pBuf, message);  
  
  MakeRequest(buf);
}
