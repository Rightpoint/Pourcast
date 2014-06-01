using System;

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
namespace Toolbox.NETMF.NET
{
    /// <summary>
    /// Simplifies usage of sockets in .NETMF
    /// </summary>
    public abstract class SimpleSocket
    {
        /// <summary>
        /// Supported protocols
        /// </summary>
        public enum SocketProtocol
        {
            /// <summary>The socket will work as a TCP Stream</summary>
            TcpStream = 1,
            /// <summary>The socket will work as a UDP Datagram</summary>
            UdpDatagram = 2
        }

        /// <summary>
        /// Possible features
        /// </summary>
        public enum SocketFeatures
        {
            /// <summary>When the socket has support for a TCP Stream</summary>
            TcpStream = 1,
            /// <summary>When the socket has support for a UDP Datagram</summary>
            UdpDatagram = 2,
            /// <summary>When the socket has support for built-in NTP</summary>
            NtpClient = 3,
            /// <summary>When the socket can also listen to a local port</summary>
            TcpListener = 4
        }

        /// <summary>
        /// Listens on the port instead of connecting remotely
        /// </summary>
        public virtual void Listen() { throw new NotImplementedException(); }

        /// <summary>
        /// Returns a timestamp from an NTP server
        /// </summary>
        /// <returns>The amount of seconds since 1 jan. 1900</returns>
        public virtual double NtpLookup()
        {
            throw new NotImplementedException();
        }

        /// <summary>When LineEnding contains data, <see cref="Receive"/> will only return data when <see cref="LineEnding"/> is reached</summary>
        public virtual string LineEnding { get; set; }

        /// <summary>
        /// Connects to the remote host
        /// </summary>
        /// <param name="Protocol">The protocol to be used</param>
        public abstract void Connect(SocketProtocol Protocol = SocketProtocol.TcpStream);

        /// <summary>
        /// Closes the connection
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Sends string data to the socket
        /// </summary>
        /// <param name="Data">The string to send</param>
        public virtual void Send(string Data)
        {
            this.SendBinary(Tools.Chars2Bytes(Data.ToCharArray()));
        }

        /// <summary>
        /// Sends binary data to the socket
        /// </summary>
        /// <param name="Data">The binary data to send</param>
        public abstract void SendBinary(byte[] Data);

        /// <summary>
        /// Returns true when connected, otherwise false
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>Returns the hostname this socket is configured for/connected to</summary>
        public abstract string Hostname { get; }
        /// <summary>Returns the port number this socket is configured for</summary>
        public abstract ushort Port { get; }

        /// <summary>
        /// Receives data from the socket
        /// </summary>
        /// <param name="Block">When true, this function will wait until there is data to return</param>
        /// <returns>The received data (may be empty)</returns>
        public abstract string Receive(bool Block = false);

        /// <summary>
        /// Receives binary data from the socket (line endings aren't used with this method)
        /// </summary>
        /// <param name="Length">The amount of bytes to receive</param>
        /// <returns>The binary data</returns>
        public abstract byte[] ReceiveBinary(int Length);

        /// <summary>
        /// Requests the amount of bytes available in the buffer
        /// </summary>
        public abstract uint BytesAvailable { get; }

        /// <summary>
        /// Checks if a feature is implemented
        /// </summary>
        /// <param name="Feature">The feature to check for</param>
        /// <returns>True if the feature is implemented</returns>
        public virtual bool FeatureImplemented(SocketFeatures Feature)
        {
            return false;
        }
    }
}
