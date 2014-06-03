using System;
using System.IO;
using System.Net;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Messaging;
using Microsoft.SPOT.Net.NetworkInformation;

namespace RightpointLabs.Pourcast.Repourter
{
    public class EthernetMessageSender : IMessageSender
    {
        public void FetchURL(Uri url)
        {
            Debug.Print("Requesting " + url.AbsoluteUri);

            var req = (HttpWebRequest)WebRequest.Create(url);
            var resp = (HttpWebResponse)req.GetResponse();
            using (resp)
            {
                Debug.Print("Http response: " + resp.StatusCode);
                if (resp.StatusCode != HttpStatusCode.NoContent)
                {
                    using (var tr = new StreamReader(resp.GetResponseStream()))
                    {
                        Debug.Print("Response: " + tr.ReadToEnd());
                    }
                }
            }
        }

        public void Initalize()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.IPAddress != "0.0.0.0")
                {
                    Debug.Print("Have IP: " + ni.IPAddress);
                    return;
                }
            }

            Debug.Print("No IP -- rebooting");
            PowerState.RebootDevice(false, 1000);
        }
    }
}
