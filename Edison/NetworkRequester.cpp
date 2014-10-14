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
    client.println();
    while(client.available() || client.connected()) {
      Serial.print(client.read());
    }
  }
  digitalWrite(_pin, HIGH);
}

