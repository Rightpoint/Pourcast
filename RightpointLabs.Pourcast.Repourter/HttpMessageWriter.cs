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
            //SendMessage(tapId);
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
            WiFlyGSX WifiModule = new WiFlyGSX(DebugMode:true);
            WifiModule.EnableDHCP();
            //var test = WiFlyGSX.AuthMode.WPA2_PSK.ToString();
            
            //WifiModule.JoinNetwork("AndroidAP");

            Debug.Print("Local IP: " + WifiModule.LocalIP);
            Debug.Print("MAC address: " + WifiModule.MacAddress);

            // Creates a socket
            var controller = "/api/repourtertest/"; // TODO: change
            //var controller = "/api/tap/";
            var action = isStart ? "startpour" : "stoppour";
            var parameters = "?tapId=" + tapId + (isStart ? "" : "&volume=" + volume);
            var request = "GET " + controller + action + parameters + " HTTP/1.1\r\n";
            //SimpleSocket socket = new WiFlySocket("google.com", 80, WifiModule);
            SimpleSocket socket = new WiFlySocket("192.168.137.1", 80, WifiModule);
            try
            {
                // Connects to the socket
                socket.Connect();
                // Does a plain HTTP request
                socket.Send(request);
                socket.Send("Host: " + socket.Hostname + "\r\n");
                socket.Send("Connection: Close\r\n");
                socket.Send("\r\n");

                // Prints all received data to the debug window, until the connection is terminated
                //while (Socket.IsConnected)
                //{
                //    var line = Socket.Receive().Trim();
                //    if (line != "" && line != null)
                //    {
                //        Debug.Print(line);
                //    }
                //}
            }
            finally
            {
                //socket.Close();
            }
        }
    }
}
