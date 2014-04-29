using System;
using System.Threading;
using Microsoft.SPOT;
using Toolbox.NETMF.Hardware;
using Toolbox.NETMF.NET;

namespace RightpointLabs.Pourcast.Repourter
{
    public static class HttpMessageWriter
    {
        public static void SendStartAsync(int tapId)
        {
            var thread = new Thread(() => SendMessage(tapId));
            thread.Start();
        }

        public static void SendStopAsync(int tapId, double ounces)
        {
            var thread = new Thread(() => SendMessage(tapId, ounces));
            thread.Start();
        }

        private static void SendMessage(int tapId, double volume = 0.0, bool isStart = true)
        {
            WiFlyGSX WifiModule = new WiFlyGSX();
            WifiModule.EnableDHCP();
            WifiModule.JoinNetwork("The Password is 1234", 0, WiFlyGSX.AuthMode.WPA2_PSK, "I lied, it's not.");

            // Creates a socket
            SimpleSocket Socket = new WiFlySocket("/rpc-cdeclue/", 80, WifiModule);

            // Connects to the socket
            Socket.Connect();
            var controller = "/api/reportertest/"; // TODO: change
            //var controller = "/api/tap/";
            var action = isStart ? "startpourt" : "stoppour";
            var parameters = "?tapId=" + tapId + (isStart ? "" : "&volume=" + volume);
            // Does a plain HTTP request
            Socket.Send("GET " + controller + action + parameters + " HTTP/1.1\r\n");
            Socket.Send("Host: " + Socket.Hostname + "\r\n");
            Socket.Send("Connection: Close\r\n");
            Socket.Send("\r\n");

            // Prints all received data to the debug window, until the connection is terminated
            while (Socket.IsConnected)
            {
                Debug.Print(Socket.Receive());
            }

            // Closes down the socket
            Socket.Close();
        }
    }
}
