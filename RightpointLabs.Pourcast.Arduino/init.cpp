#include <time.h>
#include <sys/time.h>
#include "NTPClient.h"

void initWifi(const char *ssid, const char *pass) {
    // Attempt to connect to Wifi network:
    Serial.print("\r\n\r\nAttempting to connect to SSID: ");
    Serial.println(ssid);

    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:
    int status = WiFi.begin(ssid, pass);

    while (WiFi.status() != WL_CONNECTED) {
        delay(500);
        Serial.print(".");
    }

    Serial.println("\r\nConnected to wifi");

    IPAddress ip = WiFi.localIP();
    Serial.print("IP Address: ");
    Serial.println(ip);
}

///////////////////////////////////////////////////////////////////////////////////////////////////
void initTime() {
    Adafruit_WINC1500UDP     _udp;


    time_t epochTime = (time_t)-1;

    NTPClient ntpClient;
    ntpClient.begin();

    while (true) {
        epochTime = ntpClient.getEpochTime("0.pool.ntp.org");

        if (epochTime == (time_t)-1) {
            Serial.println("Fetching NTP epoch time failed! Waiting 5 seconds to retry.");
            delay(5000);
        } else {
            Serial.print("Fetched NTP epoch time is: ");
            Serial.println(epochTime);
            break;
        }
    }
    
    ntpClient.end();

    struct timeval tv;
    tv.tv_sec = epochTime;
    tv.tv_usec = 0;

    settimeofday(&tv, NULL);
}


