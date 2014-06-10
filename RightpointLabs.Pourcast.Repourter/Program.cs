using System;
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
            var sf800pulseConfig = new PulseConfig()
            {
                PulsesPerOunce = PulseConfig.SF800_PULSES_PER_OUNCE,
                PourStoppedDelay = 250,
                PulsesPerStoppedExtension = 50,
                PulsesPerPouring = 250,
            };
            var config = new Config
            {
                Connectivity = new ConnectivityConfig
                {
                    //Wifi = new WifiConfig
                    //{
                    //    Enabled = true,
                    //    SSID = "Rightpoint",
                    //    Password = "ChangeThis",
                    //    SecurityMode = Toolbox.NETMF.Hardware.WiFlyGSX.AuthMode.MixedWPA1_WPA2
                    //},
                    Ethernet = new EthernetConfig
                    {
                        Enabled = true
                    },
                    BaseUrl = "http://pourcast.labs.rightpoint.com/api/Tap/",
                    HttpLight = Pins.GPIO_PIN_D9
                },
                Taps = new[]
                {
                    new TapConfig
                    {
                        Input = Pins.GPIO_PIN_D13, 
                        Light = Pins.GPIO_PIN_D11, 
                        TapId = "535c61a951aa0405287989ec",
                        PulseConfig = sf800pulseConfig,
                    },
                    new TapConfig
                    {
                        Input = Pins.GPIO_PIN_D12, 
                        Light = Pins.GPIO_PIN_D10, 
                        TapId = "537d28db51aa04289027cde5",
                        PulseConfig = sf800pulseConfig,
                    },
                },
                WatchdogCheckInterval = new TimeSpan(0, 15, 0)
            };

#if true
            // a configuration for testing with a local server, buttons, and even the emulator
            var localButtonConfig = new PulseConfig()
            {
                PulsesPerOunce = 0.1,
                PourStoppedDelay = 1000,
                PulsesPerStoppedExtension = 1,
                PulsesPerPouring = 5,
            };
            config = new Config
            {
                Connectivity = new ConnectivityConfig
                {
                    Wifi = new WifiConfig()
                               {
                                   Enabled = true,
                                   SSID = "XXX",
                                   Password = "XXX",
                                   SecurityMode = Toolbox.NETMF.Hardware.WiFlyGSX.AuthMode.MixedWPA1_WPA2,
                                   DebugMode = true,
                               },
                    BaseUrl = "http://192.168.25.107:23456/api/Tap/",
#if true
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
                        TapId = "539638b03885a838541b880c",
                        PulseConfig = localButtonConfig,
                    },
                    new TapConfig
                    {
                        Input = Cpu.Pin.GPIO_Pin4, // emulator down
                        Light = Cpu.Pin.GPIO_Pin3,
                        TapId = "539638bd3885a838541b8810",
                        PulseConfig = localButtonConfig,
                    },
#endif
                },
                WatchdogCheckInterval = new TimeSpan(0, 15, 0)
            };
#endif
            // END CONFIG

            // turn on the tap lights on - they'll turn off as they initialize
            LightsOn(config.Taps);
            // turn the HTTP light *off*, it'll go on as it initializes
            LightOff(config.Connectivity.HttpLight);

            var sender = BuildMessageSender(config.Connectivity);
            sender.Initalize();

            HttpMessageWriter writer = null;
            // first watchdog makes sure we send *something* every watchdogCheckInterval.  Second reboots us 30s later if the message hasn't been sent yet (ie. if networking dies)
            // sending *any* message resets both watchdogs
            var heartbeatWatchdog = new Watchdog(config.WatchdogCheckInterval, true, () => writer.SendHeartbeatAsync());
            var rebootWatchdog = new RebootWatchdog(config.WatchdogCheckInterval + new TimeSpan(0, 0, 30));

            writer = new HttpMessageWriter(sender, config.Connectivity.BaseUrl, new OutputPort(config.Connectivity.HttpLight, true), new[] { heartbeatWatchdog, rebootWatchdog });
            var sensors = new FlowSensor[config.Taps.Length];
            for (var i = 0; i < config.Taps.Length; i++)
            {
                var tapConfig = config.Taps[i];
                sensors[i] = new FlowSensor(
                    new InterruptPort(tapConfig.Input, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh),
                    new OutputPort(tapConfig.Light, false),
                    writer,
                    tapConfig.TapId,
                    tapConfig.PulseConfig);
            }

            heartbeatWatchdog.Start();
            rebootWatchdog.Start();
            Debug.Print("Starting");

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
