using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using AForge.Imaging.Filters;
using RightpointLabs.Pourcast.Domain.Services;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    public class ImageCleanupService : IImageCleanupService
    {
        public string CleanUpImage(string rawDataUrl)
        {
            string contentType;
            var data = GetDataFromUrl(rawDataUrl, out contentType);
            using (var ms = new MemoryStream(data))
            {
                var image = (Bitmap)Bitmap.FromStream(ms);
                new HistogramEqualization().ApplyInPlace(image);
                using (var ms2 = new MemoryStream())
                {
                    image.Save(ms2, ImageFormat.Jpeg);
                    return string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(ms2.ToArray()));
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
