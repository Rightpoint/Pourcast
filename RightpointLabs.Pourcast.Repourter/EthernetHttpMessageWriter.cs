using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

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
            using(var tr = new StreamReader(resp.GetResponseStream()))
            {
                Debug.Print("Response: " + tr.ReadToEnd());
            }
        }

        public bool Start()
        {
            foreach(var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(ni.IPAddress != "0.0.0.0")
                {
                    Debug.Print("Have IP: " + ni.IPAddress);
                    StartThread();
                    return true;
                }
            }

            return false;
        }
    }
}
