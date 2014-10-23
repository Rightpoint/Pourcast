
class NetworkRequester  {
  public:
    NetworkRequester(const char* host, byte pin, Print* debug);
    void MakeRequest(const char* url);
    void LogMessage(const char* message);
    void LogMessage(const __FlashStringHelper* message);
    void Heartbeat();
  private:
    const char* _host;
    byte _pin;
    Print* _debug;
};
