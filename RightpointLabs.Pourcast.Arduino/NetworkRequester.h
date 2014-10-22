
class NetworkRequester  {
  public:
    NetworkRequester(const char* host, byte pin);
    void MakeRequest(const char* url);
    void LogMessage(const char* message);
    void LogMessage(const __FlashStringHelper* message);
    void Heartbeat();
  private:
    const char* _host;
    byte _pin;
};
