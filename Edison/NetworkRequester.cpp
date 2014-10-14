#include "Arduino.h"
#include "NetworkRequester.h"
#include <WiFi.h>
#include <WiFiClient.h>

NetworkRequester::NetworkRequester(const char* host, int port, byte pin) { 
  _host = host;
  _port = port;
  _pin = pin;
  pinMode(_pin, OUTPUT);
  digitalWrite(_pin, HIGH);
}

void NetworkRequester::MakeRequest(String url){
  digitalWrite(_pin, LOW);
  Serial.println(url);
  WiFiClient client;
  if (client.connect(_host, _port)) {
    client.println("GET " + url + " HTTP/1.0");
    client.println("HOST: " + (String)_host);
    client.println();

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
