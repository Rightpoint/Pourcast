using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using AForge.Imaging.Filters;
using log4net;
using RightpointLabs.Pourcast.Domain.Services;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    public class ImageCleanupService : IImageCleanupService
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string CleanUpImage(string rawDataUrl, out string intermediateUrl)
        {
            intermediateUrl = null;
            var cs = new FaceHaarCascade();
            var detector = new HaarObjectDetector(cs, 30)
            {
                SearchMode = ObjectDetectorSearchMode.Average,
                ScalingMode = ObjectDetectorScalingMode.SmallerToGreater,
                ScalingFactor = 1.5f,
                UseParallelProcessing = true,
                Suppression = 2
            };

            string contentType;
            var data = GetDataFromUrl(rawDataUrl, out contentType);
            using (var ms = new MemoryStream(data))
            {
                var image = (Bitmap)Bitmap.FromStream(ms);
                var faces = detector.ProcessFrame(image);

                if (faces.Length > 0)
                {
                    var intermediateImage = new Bitmap(image);
                    new RectanglesMarker(faces, Color.Red).ApplyInPlace(intermediateImage);

                    var boundary = 80;
                    var x1 = Math.Max(0, faces.Min(i => i.Left) - boundary);
                    var y1 = Math.Max(0, faces.Min(i => i.Top) - boundary);
                    var x2 = Math.Min(image.Width, faces.Max(i => i.Right) + boundary);
                    var y2 = Math.Min(image.Height, faces.Max(i => i.Bottom) + boundary);

                    var newBoundingBox = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                    new RectanglesMarker(new [] { newBoundingBox }, Color.Blue).ApplyInPlace(intermediateImage);

                    using (var ms2 = new MemoryStream())
                    {
                        intermediateImage.Save(ms2, ImageFormat.Jpeg);
                        intermediateUrl = string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(ms2.ToArray()));
                    }

                    image = new Crop(newBoundingBox).Apply(image);
                }

                new ContrastStretch().ApplyInPlace(image);
                using (var ms2 = new MemoryStream())
                {
                    image.Save(ms2, ImageFormat.Jpeg);
                    var newDataUrl = string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(ms2.ToArray()));


                    return newDataUrl;
                }
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
