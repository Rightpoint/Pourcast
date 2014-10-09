//------------------------------------------------------------------------------
// SerialPortEx.cs
//
// Extension of SerialPort class for .NET Micro Framework
//
// http://bansky.net/blog
//
// This code was written by Pavel Bansky. It is released under the terms of 
// the Creative Commons "Attribution 3.0 Unported" license.
// http://creativecommons.org/licenses/by/3.0/
//
// Modified to compile for the .Net Micro 4.3 SDK
//------------------------------------------------------------------------------


using System;
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Bansky.Net
{
    /// <summary>
    /// Extension of SerialPort class
    /// </summary>
    public class SerialPortEx : IDisposable
    {
        private SerialPort port;

        /// <summary>
        /// Initializes a new instance of the SerialPort class.
        /// </summary>
        /// <param name="configuration">Serial port configuartion</param>
        public SerialPortEx(SerialPort serialPort)
        {
            this.port = serialPort;
            this.Encoding = new System.Text.UTF8Encoding();
            this.NewLine = "\n";
        }

        /// <summary>
        /// Writes a specified number of bytes to an output buffer at the specified offset.
        /// </summary>
        /// <param name="buffer">The byte array to write the output to.</param>
        /// <param name="offset">The offset in the buffer array to begin writing.</param>
        /// <param name="count">The number of bytes to write.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            this.port.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes the parameter string to the output. 
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        public void Write(string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            if (text.Length > 0)
            {
                byte[] data = this.Encoding.GetBytes(text);
                Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer. 
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        public void WriteLine(string text)
        {
            Write(text + this.NewLine);
        }

        /// <summary>
        /// Reads a number of bytes from the SerialPort input buffer and writes those bytes into a byte array at the specified offset.
        /// </summary>
        /// <param name="buffer">The byte array to write the output to. </param>
        /// <param name="offset">The offset in the buffer array to begin writing. </param>
        /// <param name="count">The number of bytes to write.</param>
        /// <returns>The number of bytes read.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            return this.port.Read(buffer, offset, count);
        }

        /// <summary>
        /// Reads up to the NewLine value in the input buffer
        /// </summary>
        /// <returns>The contents of the input buffer up to the first occurrence of a NewLine value.</returns>
        public string ReadLine()
        {
            return ReadTo(this.NewLine);
        }

        /// <summary>
        /// Reads a string up to the specified value in the input buffer.
        /// </summary>
        /// <param name="value">A value that indicates where the read operation stops.</param>
        /// <returns>The contents of the input buffer up to the specified value.</returns>
        public string ReadTo(string value)
        {
            string textArrived = string.Empty;

            // Arguments check
            if (value == null)
                throw new ArgumentNullException();

            if (value.Length == 0)
                throw new ArgumentException();

            // This is for speed performance (in loops below)
            byte[] byteValue = Encoding.GetBytes(value);
            int byteValueLen = byteValue.Length;

            bool flag = false;
            // +1 because of two byte characters
            byte[] buffer = new byte[byteValueLen + 1];

            // Read until pattern or timeout
            do
            {
                int bytesRead;
                int bufferIndex = 0;
                Array.Clear(buffer, 0, buffer.Length);

                // Read data until the buffer size is less then pattern.Length
                // or last char in pattern is received
                do
                {
                    bytesRead = port.Read(buffer, bufferIndex, 1);
                    bufferIndex += bytesRead;

                    // if nothing was read (timeout), we will return null
                    if (bytesRead <= 0)
                    {
                        return null;
                    }

                }
                while ((bufferIndex < byteValueLen + 1)
                        && (buffer[bufferIndex - 1] != byteValue[byteValueLen - 1]));

                // Decode received bytes into chars and then into string
                char[] charData = Encoding.GetChars(buffer);

                for (int i = 0; i < charData.Length; i++)
                    textArrived += charData[i];


                flag = true;

                // This is very important!! Bytes received can be zero-length string.
                // For example 0x00, 0x65, 0x66, 0x67, 0x0A will be decoded as empty string.
                /// So this condition is not a burden!
                if (textArrived.Length > 0)
                {
                    // check whether the end pattern is at the end
                    for (int i = 1; i <= value.Length; i++)
                    {
                        if (value[value.Length - i] != textArrived[textArrived.Length - i])
                        {
                            flag = false;
                            break;
                        }
                    }
                }

            } while (!flag);

            // chop end pattern
            if (textArrived.Length >= value.Length)
                textArrived = textArrived.Substring(0, textArrived.Length - value.Length);

            return textArrived;
        }

        /// <summary>
        /// Clears all buffers and causes any buffered data to be written to the SerialPort
        /// </summary>
        public void Flush()
        {
            this.port.Flush();
        }

        #region IDisposable Members

        public void Dispose()
        {
            port.Dispose();
            port = null;
        }

        #endregion

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        public System.Text.Encoding Encoding;

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine methods.
        /// </summary>
        public string NewLine;

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a read operation does not finish.
        /// </summary>
        public int ReadTimeout;
    }

}