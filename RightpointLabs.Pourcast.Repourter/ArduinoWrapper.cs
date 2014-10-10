using System;
using System.IO.Ports;
using System.Threading;
using Bansky.Net;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public class ArduinoWrapper
    {
        private readonly SerialPort _port;
        private readonly ILogger _logger;

        public ArduinoWrapper(SerialPort port, ILogger logger)
        {
            _port = port;
            _logger = logger;
        }

        public void Start()
        {
            var thread = new Thread(ReadMethod);
            thread.Start();
        }

        private void ReadMethod()
        {
            _port.Open();
            var exPort = new SerialPortEx(_port);
            var alive = 0;
            var isAlive = false;

            while (true)
            {
                while (!_port.IsOpen)
                {
                    try
                    {
                        _port.Open();
                        _logger.Log("Opened serial port");
                    }
                    catch (Exception ex)
                    {
                        _logger.Log("Error opening serial port: " + ex.Message);
                        Thread.Sleep(1000);
                    }
                }
                try
                {
                    var message = exPort.ReadLine();
                    message = message.TrimEnd('\r', '\n');
                    if (message.Trim() == "")
                    {
                        // blank message basically means we're just not connected
                        continue;
                    }

                    Debug.Print("Arduino message: " + message);
                    var parts = message.Split(' ');
                    if (message.IndexOf("START ") == 0 && parts.Length == 3)
                    {
                        OnStartPour(new TapEventArgs(int.Parse(parts[1]), int.Parse(parts[2])));
                    }
                    else if (message.IndexOf("CONTINUE ") == 0 && parts.Length == 3)
                    {
                        OnContinuePour(new TapEventArgs(int.Parse(parts[1]), int.Parse(parts[2])));
                    }
                    else if (message.IndexOf("STOP ") == 0 && parts.Length == 3)
                    {
                        OnStopPour(new TapEventArgs(int.Parse(parts[1]), int.Parse(parts[2])));
                    }
                    else if (message.IndexOf("IGNORE ") == 0 && parts.Length == 3)
                    {
                        OnIgnorePour(new TapEventArgs(int.Parse(parts[1]), int.Parse(parts[2])));
                    }
                    else if (message.IndexOf("ALIVE") == 0 && parts.Length == 1)
                    {
                        OnAlive(EventArgs.Empty);
                        if (!isAlive)
                        {
                            isAlive = true;
                            _logger.Log("Arduino has started up");
                        }
                        alive++;
                        alive %= 100;
                        if (alive == 0)
                        {
                            _logger.Log("Got 100 alives from Arduino");
                        }
                    }
                    else
                    {
                        _logger.Log("Unknown message: " + message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log("Error reading/processing message from serial port: " + ex.Message);
                    Thread.Sleep(200);
                }
            }
        }

        public class TapEventArgs
        {
            public TapEventArgs(int tapNumber, int pulseCount)
            {
                TapNumber = tapNumber;
                PulseCount = pulseCount;
            }

            public int TapNumber { get; private set; }
            public int PulseCount { get; private set; }
        }

        public delegate void TapEventHandler(object sender, TapEventArgs args);
        public delegate void EventHandler(object sender, EventArgs args);

        public event TapEventHandler StartPour;
        public event TapEventHandler IgnorePour;
        public event TapEventHandler ContinuePour;
        public event TapEventHandler StopPour;
        public event EventHandler Alive;

        #region Event callers
        protected virtual void OnStartPour(TapEventArgs args)
        {
            var handler = StartPour;
            if (handler != null) handler(this, args);
        }

        protected virtual void OnIgnorePour(TapEventArgs args)
        {
            var handler = IgnorePour;
            if (handler != null) handler(this, args);
        }

        protected virtual void OnContinuePour(TapEventArgs args)
        {
            var handler = ContinuePour;
            if (handler != null) handler(this, args);
        }

        protected virtual void OnStopPour(TapEventArgs args)
        {
            var handler = StopPour;
            if (handler != null) handler(this, args);
        }

        protected virtual void OnAlive(EventArgs args)
        {
            var handler = Alive;
            if (handler != null) handler(this, args);
        }
        #endregion
    }
}
