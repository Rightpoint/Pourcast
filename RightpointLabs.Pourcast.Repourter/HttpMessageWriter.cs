using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Toolbox.NETMF.Hardware;
using Toolbox.NETMF.NET;

namespace RightpointLabs.Pourcast.Repourter
{
    public class WifiHttpMessageWriter : HttpMessageWriterBase
    {
        private WiFlyGSX WifiModule = new WiFlyGSX(DebugMode: true);

        public bool Start(string ssid, string password, WiFlyGSX.AuthMode securityMode)
        {
            WifiModule.EnableDHCP();

            WifiModule.JoinNetwork(ssid, 0, securityMode, password, 1);

            for (var i = 0; i < 10 && WifiModule.LocalIP == "0.0.0.0"; i++)
            {
                Thread.Sleep(1000);
            }

            Debug.Print("Local IP: " + WifiModule.LocalIP);
            Debug.Print("MAC address: " + WifiModule.MacAddress);

            if (WifiModule.LocalIP == "0.0.0.0")
            {
                return false;
            }

            new Thread(SendMessages).Start();
            return true;
        }

        public void Stop()
        {
            _queue.Add(null);
        }

        protected override void SendMessage(Message message)
        {
            // Creates a socket
            var controller = "/api/repourtertest/"; // TODO: change
            //var controller = "/api/tap/";
            var action = message.IsStart ? "startpour" : "stoppour";
            var parameters = "?tapId=" + message.TapId + (message.IsStart ? "" : "&volume=" + message.Volume);
            var request = "GET " + controller + action + parameters + " HTTP/1.1\r\n";
            //SimpleSocket socket = new WiFlySocket("google.com", 80, WifiModule);
            //SimpleSocket socket = new WiFlySocket("pourcast.labs.rightpoint.com", 80, WifiModule);
            SimpleSocket socket = new WiFlySocket("192.168.100.114", 80, WifiModule);
            
            try
            {
                // Connects to the socket
                socket.Connect();
                // Does a plain HTTP request
                socket.Send(request);
                socket.Send("Host: " + "pourcast.labs.rightpoint.com" + "\r\n");
                socket.Send("Connection: Close\r\n");
                socket.Send("\r\n");

                // Prints all received data to the debug window, until the connection is terminated
                while (socket.IsConnected)
                {
                    var line = socket.Receive().Trim();
                    if (line != "" && line != null)
                    {
                        Debug.Print(line);
                    }
                }
            }
            finally
            {
                socket.Close();
            }
        }
    }
}
