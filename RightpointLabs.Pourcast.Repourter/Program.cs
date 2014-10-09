﻿using System;
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using RightpointLabs.Pourcast.Repourter.Configuration;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Program
    {
        /// <summary>
        /// Main method runs on startup
        /// </summary>
        public static void Main()
        {
            while (true)
            {
                try
                {
                    RealMain();
                }
                catch (Exception ex)
                {
                    Debug.Print("Crashing...");
                    Debug.Print(ex.ToString());
                }
                Debug.Print("Exited RealMain - rebooting");
                PowerState.RebootDevice(false, 1);
            }
        }

        /// <summary>
        /// Main method runs on startup
        /// </summary>
        public static void RealMain()
        {
            Debug.EnableGCMessages(true);
            // START CONFIG
            var config = new Config
            {
                Connectivity = new ConnectivityConfig
                {
                    //Wifi = new WifiConfig
                    //{
                    //    Enabled = true,
                    //    SSID = "Rightpoint",
                    //    Password = "NotWorking",
                    //    SecurityMode = Toolbox.NETMF.Hardware.WiFlyGSX.AuthMode.MixedWPA1_WPA2
                    //},
                    Ethernet = new EthernetConfig
                    {
                        Enabled = true
                    },
                    BaseUrl = "http://pourcast.labs.rightpoint.com/api/",
                    HttpLight = Pins.GPIO_PIN_D11
                },
                Taps = new[]
                {
                    new TapConfig
                    {
                        PulsesPerOz = TapConfig.SF800_PULSES_PER_OUNCE,
                        TapNumber = 1,
                        Light = Pins.GPIO_PIN_D9, 
                        TapId = "535c61a951aa0405287989ec",
                    },
                    new TapConfig
                    {
                        PulsesPerOz = TapConfig.SF800_PULSES_PER_OUNCE,
                        TapNumber = 2,
                        Light = Pins.GPIO_PIN_D10, 
                        TapId = "537d28db51aa04289027cde5",
                    },
                },
                WatchdogCheckInterval = new TimeSpan(0, 5, 0)
            };

            config.Connectivity.BaseUrl = "http://192.168.25.107:57168/";
            config.Connectivity.Ethernet.Enabled = false;
#if false
            // a configuration for testing with a local server, buttons, and even the emulator
            var localButtonConfig = new PulseConfig()
            {
                PulsesPerOunce = 0.1,
                PourStoppedDelay = 1000,
                PulsesPerStoppedExtension = 3,
                PulsesPerPouring = 5,
                MinPulsesRequired = 2,
            };
            config = new Config
            {
                Connectivity = new ConnectivityConfig
                {
#if false
                    Wifi = new WifiConfig()
                               {
                                   Enabled = true,
                                   SSID = "XXX",
                                   Password = "XXX",
                                   SecurityMode = Toolbox.NETMF.Hardware.WiFlyGSX.AuthMode.MixedWPA1_WPA2,
                                   DebugMode = true,
                               },
#else
                    Ethernet = new EthernetConfig()
                    {
                        Enabled = true,
                    },
#endif
                    BaseUrl = "http://10.17.56.157:23456/api/",
#if false
                    HttpLight = Pins.GPIO_PIN_D9,
                },
                Taps = new[]
                {
                    new TapConfig
                    {
                        Input = Pins.GPIO_PIN_D13, 
                        Light = Pins.GPIO_PIN_D11, 
                        TapId = "5396779faa6179467050c33a",
                        PulseConfig = localButtonConfig,
                    },
                    new TapConfig
                    {
                        Input = Pins.GPIO_PIN_D12, 
                        Light = Pins.GPIO_PIN_D10, 
                        TapId = "539677a4aa6179467050c33d",
                        PulseConfig = localButtonConfig,
                    },
#else
                    HttpLight = Cpu.Pin.GPIO_Pin6,
                },
                Taps = new[]
                {
                    new TapConfig
                    {
                        Input = Cpu.Pin.GPIO_Pin2, // emulator up
                        Light = Cpu.Pin.GPIO_Pin1,
                        TapId = "53cd261e3885a87ba4b43ea0",
                        PulseConfig = localButtonConfig,
                    },
                    new TapConfig
                    {
                        Input = Cpu.Pin.GPIO_Pin4, // emulator down
                        Light = Cpu.Pin.GPIO_Pin3,
                        TapId = "53cd26223885a87ba4b43ea2",
                        PulseConfig = localButtonConfig,
                    },
#endif
                },
                WatchdogCheckInterval = new TimeSpan(0, 15, 0)
            };
#endif
            // END CONFIG

            Debug.Print("Configuration loaded");
            // turn on the tap lights on - they'll turn off as they initialize
            LightsOn(config.Taps);
            // turn the HTTP light *off*, it'll go on as it initializes
            LightOff(config.Connectivity.HttpLight);

            Debug.Print("Lights initialized");

            var sender = BuildMessageSender(config.Connectivity);
            sender.Initalize();

            HttpMessageWriter writer = null;
            var logger = new Logger();
            // first watchdog makes sure we send *something* every watchdogCheckInterval.  Second reboots us 30s later if the message hasn't been sent yet (ie. if networking dies)
            // sending *any* message resets both watchdogs
            Watchdog heartbeatWatchdog = null;
            heartbeatWatchdog = new Watchdog(config.WatchdogCheckInterval, false, () =>
            {
                writer.SendHeartbeatAsync();
                heartbeatWatchdog.Reset();
            });
            var rebootWatchdog = new RebootWatchdog(config.WatchdogCheckInterval + new TimeSpan(0, 0, 30), logger);

            var arduino = new ArduinoWrapper(new SerialPort("COM1", 9600));
            writer = new HttpMessageWriter(sender, config.Connectivity.BaseUrl, new OutputPort(config.Connectivity.HttpLight, true), logger, new[] { rebootWatchdog });
            logger.SetWriter(writer);
            var sensors = new FlowSensor[config.Taps.Length];
            for (var i = 0; i < config.Taps.Length; i++)
            {
                var tapConfig = config.Taps[i];
                sensors[i] = new FlowSensor(
                    arduino,
                    new OutputPort(tapConfig.Light, false),
                    writer,
                    tapConfig.TapId,
                    tapConfig.TapNumber,
                    tapConfig.PulsesPerOz,
                    logger);
            }

            heartbeatWatchdog.Start();
            rebootWatchdog.Start();
            arduino.Start();
            logger.Log("Starting");
            writer.SendHeartbeatAsync();

            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static IMessageSender BuildMessageSender(ConnectivityConfig config)
        {
            IMessageSender sender = null;
#if WIFI
            if (null != config.Wifi && config.Wifi.Enabled)
            {
                if (null != sender)
                {
                    throw new ApplicationException("Cannot configure multiple connectivity methods");
                }
                sender = new WifiMessageSender(config.Wifi.SSID, config.Wifi.Password, config.Wifi.SecurityMode, config.Wifi.DebugMode);
            }
#endif
#if ETHERNET
            if (null != config.Ethernet && config.Ethernet.Enabled)
            {
                if (null != sender)
                {
                    throw new ApplicationException("Cannot configure multiple connectivity methods");
                }
                sender = new EthernetMessageSender();
            }
#endif
#if NO_NETWORKING
            if (null == sender)
            {
                Debug.Print("Using dummy message sender - requests will *not* be sent anywhere");
                sender = new NullMessageSender();
            }
#endif
            if (null == sender)
            {
                throw new ApplicationException("No valid networking stack selected");
            }
            return sender;
        }

        /// <summary>
        /// Turns on the lights for the passed stuff
        /// </summary>
        private static void LightsOn(TapConfig[] taps)
        {
            foreach (var tap in taps)
            {
                using (var p = new OutputPort(tap.Light, true))
                {
                    p.Write(true);
                }
            }
        }

        private static void LightOff(Cpu.Pin httpLight)
        {
            using (var p = new OutputPort(httpLight, false))
            {
                p.Write(false);
            }
        }
    }
}
