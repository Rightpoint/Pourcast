using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var people = "".Split(',');
            var svc = new FaceRecognitionService("7c1d51ce2ff74be58ecc1668a32e6172", "63af4f447bec4d889a5a23d3a7928506", "rpphotos", people, new ImageCleanupService());
            string intermediateUrl;
            string finalUrl;
            var users = svc.ProcessImage(rawImage, out intermediateUrl, out finalUrl);
            if(!string.IsNullOrEmpty(intermediateUrl))
                Console.WriteLine(intermediateUrl);
            if (!string.IsNullOrEmpty(finalUrl))
                Console.WriteLine(finalUrl);
            Console.WriteLine("Users: " + string.Join(", ", users));
        }
    }
}
