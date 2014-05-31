using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public class EthernetHttpMessageWriter : HttpMessageWriterBase
    {
        protected override void SendMessage(Message message)
        {
            var uri = new Uri(
                "http://pourcast.labs.rightpoint.com/api/repourtertest/" + 
                (message.IsStart ? "startpour" : "stoppour") +
                "?tapId=" + message.TapId + 
                (message.IsStart ? "" : "&volume=" + message.Volume)
                );

            Debug.Print("Requesting " + uri.AbsoluteUri);

            var req = (HttpWebRequest)WebRequest.Create(uri);
            var resp = req.GetResponse();
            using(var s = resp.GetResponseStream())
            {
                byte[] buffer = new byte[16 * 1024];
                var read = s.Read(buffer, 0, buffer.Length); // betting we don't need more than 16k to read the response because I'm too lazy to write array-merge code
                Debug.Print("Response: " + new string(Encoding.UTF8.GetChars(buffer, 0, read)));
            }
        }

        public bool Start()
        {
            new Thread(SendMessages).Start();
            return true;
        }

        public void Stop()
        {
            _queue.Add(null);
        }
    }
}
