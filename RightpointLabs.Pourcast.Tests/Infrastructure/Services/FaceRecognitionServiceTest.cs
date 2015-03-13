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
            var svc = new FaceRecognitionService("", "", "", new ImageCleanupService());
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
