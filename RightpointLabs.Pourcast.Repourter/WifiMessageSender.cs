using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Toolbox.NETMF.Hardware;
using Toolbox.NETMF.NET;

namespace RightpointLabs.Pourcast.Repourter
{
    public class WifiMessageSender : IMessageSender
    {
        private readonly string _ssid;
        private readonly string _password;
        private readonly WiFlyGSX.AuthMode _securityMode;

        public WifiMessageSender(string ssid, string password, WiFlyGSX.AuthMode securityMode)
        {
            _ssid = ssid;
            _password = password;
            _securityMode = securityMode;
        }

        public void Initalize()
        {
            GetModule();
        }

        WiFlyGSX _module;
        private WiFlyGSX GetModule()
        {
            using (var initWatchdog = new Watchdog(new TimeSpan(0, 5, 0), () =>
            {
                Debug.Print("Spent 5 minutes trying to initialize, giving up....");
                PowerState.RebootDevice(false, 1000);
            }))
            {
                while (null == _module)
                {
                    _module = GetModule();
                }
                return _module;
            }
        }

        private WiFlyGSX SetupModule()
        {
            WiFlyGSX workingModule = null;

            var thread = new Thread(() =>
            {
                var module = new WiFlyGSX(DebugMode: true);
                module.EnableDHCP();

                var isConnected = false;

                for (var i = 0; i < 3 && !(isConnected = module.JoinNetwork(_ssid, 0, _securityMode, _password)); i++ )
                {
                    Thread.Sleep(1000);
                }

                Debug.Print("isConnected: " + isConnected);

                if (!isConnected)
                {
                    module.Reboot();
                    module.Dispose();
                    return;
                }

                for (var i = 0; i < 10 && module.LocalIP == "0.0.0.0"; i++)
                {
                    Thread.Sleep(1000);
                }

                Debug.Print("Local IP: " + module.LocalIP);
                Debug.Print("MAC address: " + module.MacAddress);

                if (module.LocalIP == "0.0.0.0")
                {
                    module.Reboot();
                    module.Dispose();
                    return;
                }

                workingModule = module;
            });
            thread.Start();

            using (var setupWatchdog = new Watchdog(new TimeSpan(0, 0, 5), () =>
            {
                Debug.Print("Triggering setup watchdog");
                thread.Abort();
            }))
            {
                thread.Join();
            }

            return workingModule;
        }

        public void FetchURL(Uri url)
        {
            bool success = false;
            while (!success)
            {
                var module = GetModule();

                var thread = new Thread(() =>
                {
                    var httpHost = url.Host;
                    var host = httpHost;
                    var port = (ushort)url.Port;

                    if (host == "pourcast.labs.rightpoint.com")
                        host = "192.168.100.114";

                    var request = "GET " + url.AbsolutePath + " HTTP/1.1\r\nHost: " + httpHost + "\r\nConnection: Close\r\n\r\n";
                    SimpleSocket socket = new WiFlySocket(host, port, module);

                    try
                    {
                        // Connects to the socket
                        socket.Connect();
                        // Does a plain HTTP request
                        socket.Send(request);

                        // Prints all received data to the debug window, until the connection is terminated
                        while (socket.IsConnected)
                        {
                            var line = socket.Receive().Trim();
                            if (line != "" && line != null)
                            {
                                //Debug.Print(line);
                            }
                        }
                        success = true;
                    }
                    finally
                    {
                        socket.Close();
                    }
                });
                thread.Start();

                using (var fetchWatchdog = new Watchdog(new TimeSpan(0, 0, 5), () =>
                {
                    Debug.Print("Triggering fetch watchdog");
                    thread.Abort();
                    Thread.Sleep(500);
                    module.Reboot();
                    module.Dispose();
                    _module = null;
                }))
                {
                    thread.Join();
                }
            }
        }
    }
}
