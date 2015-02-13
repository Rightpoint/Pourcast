using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using AForge.Imaging.Filters;
using log4net;
using RightpointLabs.Pourcast.Domain.Services;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    public class ImageCleanupService : IImageCleanupService
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string CleanUpImage(string rawDataUrl)
        {
            string contentType;
            var data = GetDataFromUrl(rawDataUrl, out contentType);
            using (var ms = new MemoryStream(data))
            {
                var image = (Bitmap)Bitmap.FromStream(ms);
                new ContrastStretch().ApplyInPlace(image);
                using (var ms2 = new MemoryStream())
                {
                    image.Save(ms2, ImageFormat.Jpeg);
                    var newDataUrl = string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(ms2.ToArray()));
                    log.DebugFormat("Converted {0} to {1}", rawDataUrl.Length, newDataUrl.Length);
                    log.DebugFormat("  Input: {0}", rawDataUrl);
                    log.DebugFormat("  Output: {0}", newDataUrl);
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
