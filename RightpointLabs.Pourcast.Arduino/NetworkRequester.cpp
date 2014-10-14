#include "WiFlyHttp.h"

WiFlyHttp::WiFlyHttp(const char* host, int port, byte pin) { 
  _client = new WiFlyClient(host, port);
  _pin = pin;
  pinMode(_pin, OUTPUT);
  digitalWrite(_pin, HIGH);
}

void WiFlyHttp::MakeRequest(String url){
  digitalWrite(_pin, LOW);
  Serial.println(url);
  if (_client->connect()) {
    _client->println("GET " + url + " HTTP/1.0");
    _client->println();
    while(_client->available() || _client->connected()) {
      Serial.print(_client->read());
    }
  }
  digitalWrite(_pin, HIGH);
}

