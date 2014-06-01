using System;
using Toolbox.NETMF.Hardware;

/*
 * Copyright 2012-2013 Stefan Thoolen (http://www.netmftoolbox.com/)
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
namespace Toolbox.NETMF.NET
{
    /// <summary>
    /// Simplifies usage of sockets for the WiFly module
    /// </summary>
    public class WiFlySocket : SimpleSocket
    {
        /// <summary>Reference to the network interface</summary>
        private WiFlyGSX _NetworkInterface;
        /// <summary>The remote host name</summary>
        private string _Hostname = "";
        /// <summary>The remote TCP port</summary>
        private ushort _Port = 0;

        /// <summary>
        /// Creates a new socket based on the WiFly socket TCP stack
        /// </summary>
        /// <param name="Hostname">The hostname to connect to</param>
        /// <param name="Port">The port to connect to</param>
        /// <param name="NetworkInterface">Reference to the WiFly module</param>
        public WiFlySocket(string Hostname, ushort Port, WiFlyGSX NetworkInterface)
        {
            this._Hostname = Hostname;
            this._Port = Port;
            this._NetworkInterface = NetworkInterface;
            this.LineEnding = "";
        }

        /// <summary>
        /// Returns a timestamp from an NTP server
        /// </summary>
        /// <returns>The amount of seconds since 1 jan. 1900</returns>
        public override double NtpLookup()
        {
            string ip = this._NetworkInterface.DnsLookup(this._Hostname);
            return this._NetworkInterface.NtpLookup(ip, this._Port);
        }

        /// <summary>
        /// Connects to the remote host
        /// </summary>
        /// <param name="Protocol">The protocol to be used</param>
        public override void Connect(SocketProtocol Protocol = SocketProtocol.TcpStream)
        {
            if (Protocol != SocketProtocol.TcpStream)
                throw new NotImplementedException();
            this._NetworkInterface.OpenSocket(this._Hostname, this._Port);
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public override void Close()
        {
            this._NetworkInterface.CloseSocket();
        }

        /// <summary>
        /// Sends string data to the socket
        /// </summary>
        /// <param name="Data">The string to send</param>
        public override void Send(string Data)
        {
            this._NetworkInterface.SocketWrite(Data);
        }

        /// <summary>
        /// Sends binary data to the socket
        /// </summary>
        /// <param name="Data">The binary data to send</param>
        public override void SendBinary(byte[] Data)
        {
            this.Send(new string(Tools.Bytes2Chars(Data)));
        }

        /// <summary>
        /// Returns true when connected, otherwise false
        /// </summary>
        public override bool IsConnected { get { return this._NetworkInterface.SocketConnected; } }
        /// <summary>Returns the hostname this socket is configured for</summary>
        public override string Hostname { get { return this._Hostname; } }
        /// <summary>Returns the port number this socket is configured for</summary>
        public override ushort Port { get { return this._Port; } }

        /// <summary>
        /// Receives data from the socket
        /// </summary>
        /// <param name="Block">When true, this function will wait until there is data to return</param>
        /// <returns>The received data (may be empty)</returns>
        public override string Receive(bool Block = false)
        {
            string RetValue = "";
            do
            {
                RetValue = this._NetworkInterface.SocketRead(-1, this.LineEnding);
            } while (IsConnected && Block && RetValue == "");

            return RetValue;
        }

        /// <summary>
        /// Receives binary data from the socket (line endings aren't used with this method)
        /// </summary>
        /// <param name="Length">The amount of bytes to receive</param>
        /// <returns>The binary data</returns>
        public override byte[] ReceiveBinary(int Length)
        {
            return Tools.Chars2Bytes(this._NetworkInterface.SocketRead(Length).ToCharArray());
        }

        /// <summary>
        /// Checks if a feature is implemented
        /// </summary>
        /// <param name="Feature">The feature to check for</param>
        /// <returns>True if the feature is implemented</returns>
        public override bool FeatureImplemented(SocketFeatures Feature)
        {
            switch (Feature)
            {
                case SocketFeatures.TcpStream:
                case SocketFeatures.NtpClient:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>Requests the amount of bytes available in the buffer</summary>
        public override uint BytesAvailable { get { return this._NetworkInterface.SocketBufferLength; } }
    }
}
