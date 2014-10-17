#include <WiFlySerial.h>

class NetworkRequester  {
  public:
    NetworkRequester(WiFlySerial* wiFly, const char* host, byte pin);
    void MakeRequest(const char* url);
    void LogMessage(const char* message);
    void LogMessage(const __FlashStringHelper* message);
    void Heartbeat();
  private:
    WiFlySerial* _wiFly;
    const char* _host;
    byte _pin;
};
