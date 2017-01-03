extern "C" {
  void deviceSetup(const char* connectionString);
  void deviceTeardown();
  void deviceProcess(int delay);
  void deviceSend(int timeSinceLastData, double temperature, double weight, int pulses, const char* kegId);
}

