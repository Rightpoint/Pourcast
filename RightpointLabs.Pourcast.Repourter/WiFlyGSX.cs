using System;
using System.Threading;
using System.IO.Ports;
using Microsoft.SPOT;

/*
 * Copyright 2011-2013 Stefan Thoolen (http://www.netmftoolbox.com/)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Toolbox.NETMF.Hardware
{
    /// <summary>
    /// Roving Networks WiFly GSX module driver
    /// </summary>
    /// <remarks>
    /// This class is based on the Roving Networks manual found at http://www.rovingnetworks.com/files/resources/WiFly-RN-UM-2.31-v-0.1r.pdf
    /// The class has been tested with a RN-XV board containing an RN-171 module containing the 2.27 firmware.
    /// The implementation is not 100% perfect, but it works for most applications.
    /// As mentioned in the Apache 2.0 license note above, no guarantees can be given, I just hope this code might be of help.
    /// </remarks>
    public class WiFlyGSX : IDisposable
    {
        #region "Construction and destruction"
        /// <summary>
        /// Initializes the WiFly GSX Module
        /// </summary>
        /// <param name="PortName">The serial port the module is connected to</param>
        /// <param name="BaudRate">The setup speed of the serial port</param>
        /// <param name="CommandChar">The character used to enter command mode</param>
        /// <param name="DebugMode">Enables debug mode</param>
        public WiFlyGSX(string PortName = "COM1", int BaudRate = 9600, string CommandChar = "$", bool DebugMode = false)
        {
            // Configures this client
            this._CommandMode_InitString = CommandChar + CommandChar + CommandChar;
            this.DebugMode = DebugMode;
            this._Mode = Modes.Idle;

            // Configures and opens the port
            this._SerialPort = new SerialPort(PortName, BaudRate, Parity.None, 8, StopBits.One);
            this._SerialPort.DataReceived += new SerialDataReceivedEventHandler(_SerialPort_DataReceived);
            this._SerialPort.Open();

            // Makes sure we're not in a command mode or something
            this._SerialPort_Write("exit\r");

            // Starts command mode
            this._CommandMode_Start();

            // Disables system output; we only want to receive data when we request it, so we won't get bogus data through our streams
            if (!this._CommandMode_Exec("set sys printlvl 0")) throw new SystemException(this._CommandMode_Response);
            // Disables the welcome greeting sent when we open a connection
            if (!this._CommandMode_Exec("set comm remote 0")) throw new SystemException(this._CommandMode_Response);
            // Reads out the module version
            this.ModuleVersion = this._CommandMode_GetInfo("ver");
            // Reads out the MAC address
            this.MacAddress = this._CommandMode_GetInfo("get mac").Substring(9);
            // Requests the communication options
            this._SocketOpenString = this._CommandMode_ReadValue("comm", "open");
            this._SocketCloseString = this._CommandMode_ReadValue("comm", "close");

            // Leaves command mode
            this._CommandMode_Stop();
        }

        /// <summary>
        /// Reboots the Wifi Module - recommend disposing it immediately (if this even works...)
        /// </summary>
        public void Reboot()
        {
            this._CommandMode_Start();
            this._SerialPort_Write("reboot\r");
        }

        /// <summary>Disposes this object</summary>
        public void Dispose()
        {
            this._SerialPort.Dispose();
        }
        #endregion

        #region "Network tools"
        /// <summary>
        /// Looks up the IP address of a hostname
        /// </summary>
        /// <param name="Hostname">The hostname</param>
        /// <returns>The IP address</returns>
        public string DnsLookup(string Hostname)
        {
            this._CommandMode_Start();
            string RetValue = this._CommandMode_GetInfo("lookup " + Hostname);
            this._CommandMode_Stop();
            return RetValue.Substring(RetValue.IndexOf("=") + 1);
        }

        /// <summary>
        /// Returns the amount of seconds passed since 1 jan. 1900
        /// </summary>
        /// <param name="IpAddress">The IP address of an NTP server</param>
        /// <param name="Port">The UDP port of the NTP server</param>
        /// <returns>The amount of seconds passed since 1 jan. 1900</returns>
        public double NtpLookup(string IpAddress, ushort Port = 123)
        {
            this._CommandMode_Start();
            if (!this._CommandMode_Exec("set time address " + IpAddress)) throw new SystemException(this._CommandMode_Response);
            if (!this._CommandMode_Exec("set time port " + Port.ToString())) throw new SystemException(this._CommandMode_Response);
            this._SerialPort_Write("time\r");
            string RetValue = this._CommandMode_ReadValue("t", "t", "rtc");
            this._CommandMode_Stop();

            // We get the number of seconds since 1 jan. 1970
            double Value1970 = double.Parse(RetValue);
            // But we need the number of seconds since 1 jan. 1900!!
            return (Value1970 + 2208988800);
        }

        #endregion

        #region "Network configuration"
        /// <summary>Returns the MAC address of the WiFly module</summary>
        public string MacAddress { get; protected set; }

        /// <summary>Supported wifi authentication modes</summary>
        public enum AuthMode
        {
            /// <summary>No encryption at all. Are you insane?!?!?</summary>
            Open = 0,
            /// <summary>128-bit Wired Equivalent Privacy (WEP)</summary>
            WEP_128 = 1,
            /// <summary>Wi-Fi Protected Access (WPA)</summary>
            WPA1 = 2,
            /// <summary>Mixed WPA1 &amp; WPA2-PSK</summary>
            MixedWPA1_WPA2 = 3,
            /// <summary>Wi-Fi Protected Access (WPA) II with preshared key</summary>
            WPA2_PSK = 4
        }

        /// <summary>Returns the local IP</summary>
        public string LocalIP
        {
            get
            {
                this._CommandMode_Start();
                string RetValue = this._CommandMode_ReadValue("ip", "ip");
                this._CommandMode_Stop();
                return RetValue.Substring(0, RetValue.IndexOf(":"));
            }
        }

        /// <summary>
        /// Enables DHCP
        /// </summary>
        public void EnableDHCP()
        {
            // Enterring command mode
            this._CommandMode_Start();
            // Enables DHCP
            if (!this._CommandMode_Exec("set ip dhcp 1")) throw new SystemException(this._CommandMode_Response);
            // Leaves command mode
            this._CommandMode_Stop();
        }

        /// <summary>
        /// Enables and configures a static IP address
        /// </summary>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="SubnetMask">The subnet mask</param>
        /// <param name="Gateway">The gateway</param>
        /// <param name="DNS">The DNS server</param>
        public void EnableStaticIP(string IPAddress, string SubnetMask, string Gateway, string DNS)
        {
            // Enterring command mode
            this._CommandMode_Start();
            // Configures the IP
            if (!this._CommandMode_Exec("set ip dhcp 0")) throw new SystemException(this._CommandMode_Response);
            if (!this._CommandMode_Exec("set ip address " + IPAddress)) throw new SystemException(this._CommandMode_Response);
            if (!this._CommandMode_Exec("set ip gateway " + Gateway)) throw new SystemException(this._CommandMode_Response);
            if (!this._CommandMode_Exec("set ip netmask " + SubnetMask)) throw new SystemException(this._CommandMode_Response);
            if (!this._CommandMode_Exec("set dns address " + DNS)) throw new SystemException(this._CommandMode_Response);
            // Closes down command mode
            this._CommandMode_Stop();
        }

        /// <summary>
        /// Joins a wireless network
        /// </summary>
        /// <param name="SSID">The name of the wireless network</param>
        /// <param name="Channel">The channel the AP is listening on (0 for autodetect)</param>
        /// <param name="Authentication">The method for authentication</param>
        /// <param name="Key">The shared key required to join the network (WEP / WPA)</param>
        /// <param name="KeyIndex">The index of the key (WEP only)</param>
        public bool JoinNetwork(string SSID, int Channel = 0, AuthMode Authentication = AuthMode.Open, string Key = "", int KeyIndex = 1)
        {
            var retVal = false;

            // Enterring command mode
            this._CommandMode_Start();
            // Configures the network
            if (!this._CommandMode_Exec("set wlan ssid " + SSID)) throw new SystemException(this._CommandMode_Response);
            if (!this._CommandMode_Exec("set wlan channel " + Channel)) throw new SystemException(this._CommandMode_Response);
            if (!this._CommandMode_Exec("set wlan auth " + Authentication.ToString())) throw new SystemException(this._CommandMode_Response);
            if (Authentication == AuthMode.WEP_128)
            {
                if (!this._CommandMode_Exec("set wlan key " + Key)) throw new SystemException(this._CommandMode_Response);
                if (!this._CommandMode_Exec("set wlan num " + KeyIndex.ToString())) throw new SystemException(this._CommandMode_Response);
            }
            else if (Authentication != AuthMode.Open)
            {
                if (!this._CommandMode_Exec("set wlan phrase " + Key)) throw new SystemException(this._CommandMode_Response);
            }

            // Actually joins the network
            if (this._CommandMode_Exec("join"))
            {
                retVal = true;
            }
            else
            {
                this._DebugPrint('D', "Failed to join network: " + this._CommandMode_Response);
            }

            // Closes down command mode
            this._CommandMode_Stop();

            return retVal;
        }
        #endregion

        #region "Streaming mode"
        /// <summary>Identifier for the beginning of a stream</summary>
        private string _SocketOpenString = "";
        /// <summary>Identifier for the end of a stream</summary>
        private string _SocketCloseString = "";

        /// <summary>Returns the remote hostname</summary>
        public string RemoteHostname { get; protected set; }
        /// <summary>Returns the remote port</summary>
        public ushort RemotePort { get; protected set; }

        /// <summary>Returns wether we're connected to a remote host</summary>
        public bool SocketConnected { get { return this._Mode == Modes.StreamingMode; } }

        /// <summary>Returns the length of the socket buffer</summary>
        public uint SocketBufferLength { get { return (uint)this._SerialPort_StreamBuffer.Length; } }

        /// <summary>
        /// Opens a TCP socket
        /// </summary>
        /// <param name="Hostname">Remote hostname</param>
        /// <param name="Port">Remote port</param>
        /// <param name="Timeout">Socket timeout in ms</param>
        public void OpenSocket(string Hostname, ushort Port, int Timeout = 5000)
        {
            if (this._SocketOpenString == "" || this._SocketCloseString == "") throw new ApplicationException("WTF?!");

            // Copies values locally
            this.RemoteHostname = Hostname;
            this.RemotePort = Port;
            // Lets start command mode first
            this._CommandMode_Start();
            // Now we open the connection
            this._Mode = Modes.Connecting;
            this._SerialPort_Write("open " + Hostname + " " + Port.ToString() + "\r");

            // Wait till we're connected
            while (Timeout > 0)
            {
                if (this.SocketConnected) break;
                Thread.Sleep(1);
                --Timeout;
            }

            // Are we timed out?
            if (!this.SocketConnected)
            {
                this._SerialPort_Write("close\r");
                this._SerialPort_Write("exit\r");
                throw new ApplicationException("Connection timed out");
            }
        }

        /// <summary>Closes a TCP socket</summary>
        public void CloseSocket()
        {
            if (this._Mode != Modes.StreamingMode) return;

            Thread.Sleep(250);
            this._SerialPort_Write(this._CommandMode_InitString);
            Thread.Sleep(250);
            this._SerialPort_Write("close\r");
            this._SerialPort_Write("exit\r");
            while (this.SocketConnected) { }
        }

        /// <summary>
        /// Writes data to the socket
        /// </summary>
        /// <param name="WriteBuffer">Data to write</param>
        public void SocketWrite(string WriteBuffer)
        {
            if (this._Mode != Modes.StreamingMode) throw new InvalidOperationException("Can't write data when not connected");
            this._SerialPort_Write(WriteBuffer);
        }

        /// <summary>
        /// Reads data from the socket
        /// </summary>
        /// <param name="Length">The amount of bytes to read (-1 is everything)</param>
        /// <param name="UntilReached">Read until this string is reached (empty is no ending)</param>
        /// <returns>The data</returns>
        public string SocketRead(int Length = -1, string UntilReached = "")
        {
            if (this._SerialPort_StreamBuffer.Length == 0) return "";

            int SendLength = Length == -1 ? this._SerialPort_StreamBuffer.Length : Length;
            if (UntilReached != "")
            {
                int Pos = this._SerialPort_StreamBuffer.IndexOf(UntilReached);
                if (Pos < 0 || Pos >= SendLength) return "";
                SendLength = Pos + UntilReached.Length;
            }

            string RetVal = this._SerialPort_StreamBuffer.Substring(0, SendLength);
            this._SerialPort_StreamBuffer = this._SerialPort_StreamBuffer.Substring(RetVal.Length);

            return RetVal;
        }
        #endregion

        #region "Serial Port code"

        /// <summary>Current state of the serial connection</summary>
        private Modes _Mode = Modes.Idle;
        /// <summary>Possible states of the serial connection</summary>
        private enum Modes
        {
            /// <summary>Not in Command Mode nor connected</summary>
            Idle = 0,
            /// <summary>In Command Mode</summary>
            CommandMode = 1,
            /// <summary>Trying to connect</summary>
            Connecting = 2,
            /// <summary>Connected</summary>
            StreamingMode = 3
        }

        /// <summary>Buffer while in Idle or Command Mode</summary>
        private string _SerialPort_TextBuffer = "";
        /// <summary>Buffer while in Stream mode</summary>
        private string _SerialPort_StreamBuffer = "";

        /// <summary>Buffer that will contain the last _SocketCloseString.length bytes</summary>
        private string _SerialPort_EndStreamCheck = "";

        /// <summary>Reference to the serial port</summary>
        private SerialPort _SerialPort;

        /// <summary>Writes raw data to the WiFly module</summary>
        /// <param name="Text">Data to write</param>
        private void _SerialPort_Write(string Text)
        {
            _DebugPrint('O', Text);
            byte[] WriteBuffer = Tools.Chars2Bytes(Text.ToCharArray());
            this._SerialPort.Write(WriteBuffer, 0, WriteBuffer.Length);
        }

        /// <summary>The WiFly module sent us data</summary>
        /// <param name="sender">Reference to this._Port</param>
        /// <param name="e">Event data</param>
        private void _SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (this._SerialPort.BytesToRead == 0) return;

            // Reads out the data and converts it to a string
            byte[] ReadBytes = new byte[this._SerialPort.BytesToRead];
            this._SerialPort.Read(ReadBytes, 0, ReadBytes.Length);
            string ReadBuffer = new string(Tools.Bytes2Chars(ReadBytes));

            // While in Streaming mode, we need to handle the data differently
            if (this._Mode == Modes.StreamingMode)
            {
                _DebugPrint('I', ReadBuffer);
                string NewBuffer = this._SerialPort_StreamBuffer + ReadBuffer;

                // Fixes the "*CLOS*" issue as described below
                this._SerialPort_EndStreamCheck += ReadBuffer;
                if (this._SerialPort_EndStreamCheck.Length > this._SocketCloseString.Length)
                    this._SerialPort_EndStreamCheck = this._SerialPort_EndStreamCheck.Substring(this._SerialPort_EndStreamCheck.Length - this._SocketCloseString.Length);

                // Do we need to leave Streaming Mode?
                int CheckPos = NewBuffer.IndexOf(this._SocketCloseString);
                if (CheckPos >= 0)
                {
                    this._SerialPort_TextBuffer = NewBuffer.Substring(CheckPos + this._SocketCloseString.Length);
                    NewBuffer = NewBuffer.Substring(0, CheckPos);
                    this._Mode = Modes.Idle;
                    _DebugPrint('D', "Left streaming mode");
                }
                // The closing string (default: "*CLOS*") is many times sent in multiple packets.
                // This causes annoying issues of connections not shutting down well.
                // This fixes that issue, but it's possible the last few bytes of the stream contain something like "*CL" or something.
                else if (_SerialPort_EndStreamCheck == this._SocketCloseString)
                {
                    this._Mode = Modes.Idle;
                    _DebugPrint('D', "Left streaming mode");
                }
                this._SerialPort_StreamBuffer = NewBuffer;

                return;
            }

            // When not in Streaming Mode we check if we need to parse text line by line
            this._SerialPort_TextBuffer += ReadBuffer;

            // Do we can/need to enter streaming mode?
            if (this._Mode == Modes.Connecting)
            {
                int CheckPos = this._SerialPort_TextBuffer.IndexOf(this._SocketOpenString);
                if (CheckPos >= 0)
                {
                    this._SerialPort_StreamBuffer = this._SerialPort_TextBuffer.Substring(CheckPos + this._SocketOpenString.Length);
                    this._SerialPort_TextBuffer = this._SerialPort_TextBuffer.Substring(0, CheckPos);
                    this._Mode = Modes.StreamingMode;
                    _DebugPrint('D', "Enterred streaming mode");
                }
            }

            // Parses all lines
            while (true)
            {
                int CheckPos = this._SerialPort_TextBuffer.IndexOf("\r\n");
                if (CheckPos < 0) break;

                string Line = this._SerialPort_TextBuffer.Substring(0, CheckPos);
                this._SerialPort_TextBuffer = this._SerialPort_TextBuffer.Substring(CheckPos + 2);
                this._SerialPort_LineReceived(Line);
            }
        }


        /// <summary>The WiFly module sent us a line of text</summary>
        /// <param name="Text">The text</param>
        private void _SerialPort_LineReceived(string Text)
        {
            _DebugPrint('I', Text + "\r\n");

            // Did we enter command mode?
            if (Text.IndexOf("CMD") > -1 && (this._Mode == Modes.Idle || this._Mode == Modes.Connecting))
            {
                this._Mode = Modes.CommandMode;
                _DebugPrint('D', "Successfully enterred the command mode");
                return;
            }

            // Is this line for Command Mode?
            if (this._Mode == Modes.CommandMode) _CommandMode_LineReceived(Text);
        }

        #endregion

        #region "Command Mode support"
        /// <summary>Returns the version of the WiFly module</summary>
        public string ModuleVersion { get; protected set; }

        /// <summary>Contains the init string to enter command mode</summary>
        private string _CommandMode_InitString = "";

        /// <summary>The WiFly module sent us a line of text during Command Mode</summary>
        /// <param name="Text">The text</param>
        private void _CommandMode_LineReceived(string Text)
        {
            // Is this an echo on Command Mode?
            if (Text.Substring(Text.Length - 1) == "\r")
            {
                // Yes it is, lets ignore it!
                _DebugPrint('D', "Last line was an echo");
                return;
            }

            // Did we left command mode?
            if (Text == "EXIT")
            {
                this._Mode = Modes.Idle;
                _DebugPrint('D', "Successfully left the command mode");
            }

            // Are we waiting for a command to complete?
            if (!this._CommandMode_ResponseComplete)
            {
                this._CommandMode_Response += Text + "\r\n";
                if (Text == "AOK" || Text.Substring(0, 3) == "ERR")
                {
                    this._CommandMode_Response = this._CommandMode_Response.TrimEnd();
                    this._CommandMode_ResponseComplete = true;
                }
            }

            // Are we waiting for info?
            if (this._CommandMode_GetInfoLines > 0)
            {
                this._CommandMode_GetInfoResponse += Text + "\r\n";
                this._CommandMode_GetInfoLines--;
            }

            // Are we waiting for just a value?
            if (this._CommandMode_ReadKey != "")
            {
                string[] Values = Text.Split("=".ToCharArray(), 2);
                if (Values[0].ToLower() == this._CommandMode_ReadKey) this._CommandMode_ReadKeyValue = Values[1];
            }
        }

        // This is the amount of info lines we still require
        private int _CommandMode_GetInfoLines = 0;
        // Info lines will be temporarily stored here
        private string _CommandMode_GetInfoResponse = "";

        /// <summary>Executes a command and returns its answer</summary>
        /// <param name="Command">The command</param>
        /// <param name="Answers">The amount of lines to be expected</param>
        /// <returns>The answer</returns>
        private string _CommandMode_GetInfo(string Command, int Answers = 1)
        {
            this._CommandMode_GetInfoLines = Answers;
            this._CommandMode_GetInfoResponse = "";
            this._SerialPort_Write(Command + "\r");

            // Wait until the command is completed
            while (this._CommandMode_GetInfoLines > 0) Thread.Sleep(1);

            this._CommandMode_GetInfoResponse = this._CommandMode_GetInfoResponse.TrimEnd();

            _DebugPrint('D', "Answer: " + this._CommandMode_GetInfoResponse);

            return this._CommandMode_GetInfoResponse;
        }

        private string _CommandMode_Response = "";
        private bool _CommandMode_ResponseComplete = true;

        /// <summary>Executes a command and wait for it's response</summary>
        /// <param name="Command">The command</param>
        /// <returns>True if it returned AOK</returns>
        private bool _CommandMode_Exec(string Command)
        {
            this._CommandMode_ResponseComplete = false;
            this._CommandMode_Response = "";
            this._SerialPort_Write(Command + "\r");

            // Wait until the command is completed
            while (!this._CommandMode_ResponseComplete) Thread.Sleep(1);

            if (this._CommandMode_Response.Substring(this._CommandMode_Response.Length - 3) == "AOK")
            {
                _DebugPrint('D', "Last command is completed with success");
                return true;
            }
            else
            {
                _DebugPrint('D', "Last command is completed with an error");
                return false;
            }
        }

        private string _CommandMode_ReadKey = "";
        private string _CommandMode_ReadKeyValue = "";

        /// <summary>Reads a value from the config</summary>
        /// <param name="List">The config chapter</param>
        /// <param name="Key">The config name</param>
        /// <returns>The value</returns>
        private string _CommandMode_ReadValue(string List, string Key)
        {
            this._CommandMode_ReadKey = Key.ToLower();
            this._CommandMode_ReadKeyValue = "";
            this._SerialPort_Write("get " + List + "\r");

            while (this._CommandMode_ReadKeyValue == "") Thread.Sleep(1);

            this._CommandMode_ReadKey = "";
            return this._CommandMode_ReadKeyValue;
        }

        /// <summary>Reads a value from the config</summary>
        /// <param name="List">The config chapter</param>
        /// <param name="SubList">The config subchapter</param>
        /// <param name="Key">The config name</param>
        /// <returns>The value</returns>
        private string _CommandMode_ReadValue(string List, string SubList, string Key)
        {
            this._CommandMode_ReadKey = Key.ToLower();
            this._CommandMode_ReadKeyValue = "";
            this._SerialPort_Write("show " + List + " " + SubList + "\r");

            while (this._CommandMode_ReadKeyValue == "") Thread.Sleep(1);

            this._CommandMode_ReadKey = "";
            return this._CommandMode_ReadKeyValue;
        }

        /// <summary>
        /// Enters the command mode
        /// </summary>
        private void _CommandMode_Start()
        {
            // Can/need we to enter the command mode?
            if (this._Mode == Modes.CommandMode) return;
            if (this._Mode == Modes.StreamingMode) throw new InvalidOperationException("Can't open Command Mode while a stream is active");

            this._SerialPort_Write("close\r");
            this._SerialPort_Write("exit\r");

            // Enterring command mode
            Thread.Sleep(250);
            this._SerialPort_Write(this._CommandMode_InitString);
            Thread.Sleep(250);

            // Wait until we actually enterred the command mode
            while (this._Mode != Modes.CommandMode)
                Thread.Sleep(1);
        }

        /// <summary>
        /// Leaves the command mode
        /// </summary>
        private void _CommandMode_Stop()
        {
            // Can we leave command mode?
            if (this._Mode != Modes.CommandMode) throw new InvalidOperationException("Can't stop Command Mode when not in Command Mode");

            // Exits command mode
            this._SerialPort_Write("exit\r");

            // Wait until we actually left the command mode
            while (this._Mode == Modes.CommandMode)
                Thread.Sleep(1);
        }
        #endregion

        #region "Debugging"
        /// <summary>When true, debugging is enabled</summary>
        public bool DebugMode { get; set; }

        /// <summary>When <see cref="DebugMode"/> is true, this event can be used to receive the debug text. If the event isn't defined, all data will be sent with Debug.Print</summary>
        //public event Toolbox.NETMF.Tools.StringEventHandler DebugReceived;

        /// <summary>Does some logging</summary>
        /// <param name="Flag">Type of data: (I)ncoming / (O)utgoing / (D)ebug</param>
        /// <param name="Text">Text to debug</param>
        private void _DebugPrint(char Flag, string Text)
        {
            if (!this.DebugMode) return;
            //if (this.DebugReceived == null)
                Debug.Print(Flag + ": " + Tools.Escape(Text));
            //else
                //this.DebugReceived(Flag + ": " + Tools.Escape(Text), DateTime.Now);
        }
        #endregion

    }
}
