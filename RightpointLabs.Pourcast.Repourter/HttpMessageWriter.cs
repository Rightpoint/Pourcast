using System;
using System.Threading;
using Microsoft.SPOT;
using Toolbox.NETMF.Hardware;
using Toolbox.NETMF.NET;

namespace RightpointLabs.Pourcast.Repourter
{
    public static class HttpMessageWriter
    {
        public static void SendMessageAsync(string tapId, double ounces)
        {
            var thread = new Thread(() => SendMessage(tapId, ounces));
            thread.Start();
        }

        private static void SendMessage(string tapId, double ounces)
        {
            WiFlyGSX WifiModule = new WiFlyGSX();
            WifiModule.EnableDHCP();
            WifiModule.JoinNetwork("Netduino");

            // Creates a socket
            SimpleSocket Socket = new WiFlySocket("www.netmftoolbox.com", 80, WifiModule);

            // Connects to the socket
            Socket.Connect();

            // Does a plain HTTP request
            Socket.Send("GET /helloworld/ HTTP/1.1\r\n");
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
