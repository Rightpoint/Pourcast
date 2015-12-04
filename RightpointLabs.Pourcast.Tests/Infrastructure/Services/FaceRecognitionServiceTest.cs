using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RightpointLabs.Pourcast.Domain.Services;
using RightpointLabs.Pourcast.Infrastructure.Services;

namespace RightpointLabs.Pourcast.Tests.Infrastructure.Services
{
    /// <summary>
    /// Disabled test class to help debug image processing services
    /// </summary>
    //[TestClass]
    public class FaceRecognitionServiceTest
    {
        //[TestMethod]
        public void TestFaces()
        {
            var rawImage = @"";
            var svc = new FaceRecognitionService("", "", "", new ImageCleanupService(), null);
            string intermediateUrl;
            string finalUrl;
            bool addedOverlay;
            var users = svc.ProcessImage(rawImage, out intermediateUrl, out finalUrl, out addedOverlay);
            if(!string.IsNullOrEmpty(intermediateUrl))
                Console.WriteLine(intermediateUrl);
            if (!string.IsNullOrEmpty(finalUrl))
                Console.WriteLine(finalUrl);
            Console.WriteLine("Users: " + string.Join(", ", users) + ", added overlay: " + addedOverlay);
        }

        //[TestMethod]
        public void TestOverlay()
        {
	        var files = Directory.GetFiles(@"C:\Users\jrupp\Desktop\mustaches");
	
	        var apiKey="";
	        var apiSecret="";
	        var ns="";
	
            var rawDataUrl = string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(File.ReadAllBytes(@"C:\Users\jrupp\Desktop\out\source1.png")));
	
	        var i = 0;
	        var outBase = @"C:\Users\jrupp\Desktop\out";
	        foreach(var file in files) {
		        var svc = new FaceRecognitionService(apiKey, apiSecret, ns, null, new FakeOverlay(file));
		        string intermediateUrl, finalUrl, contentType;
		        bool addedOverlay;
		        svc.ProcessImage(rawDataUrl, out intermediateUrl, out finalUrl, out addedOverlay);
		        File.WriteAllBytes(Path.Combine(outBase, "img-" + (i++) + ".png"), GetDataFromUrl(finalUrl, out contentType));
	        }
        }

        class FakeOverlay : IOverlayImageProvider {
	        string _filename;
	        public FakeOverlay(string filename) {
		        _filename = filename;
	        }
	        public Image GetRandomOverlayImage() {
		        return Image.FromFile(_filename);
	        }
        }

        private byte[] GetDataFromUrl(string dataUrl, out string contentType)
        {
          // https://gist.github.com/vbfox/484643
          var match = Regex.Match(dataUrl, @"data:image/(?<type>.+?);base64,(?<data>.+)");
          var type = match.Groups["type"].Value;
          var base64Data = match.Groups["data"].Value;
          var binData = Convert.FromBase64String(base64Data);

          contentType = "image/" + type;
          return binData;
        }
    }
}
