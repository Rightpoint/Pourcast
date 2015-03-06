using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Accord.Imaging.Filters;
using Accord.Statistics.Kernels;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using AForge.Imaging.Filters;
using log4net;
using RightpointLabs.Pourcast.Domain.Services;
using SkyBiometry.Client.FC;
using Point = SkyBiometry.Client.FC.Point;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    public class FaceRecognitionService : IFaceRecognitionService
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _tagNamespace;
        private readonly IImageCleanupService _imageCleanupService;

        public FaceRecognitionService(string apiKey, string apiSecret, string tagNamespace, IImageCleanupService imageCleanupService)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _tagNamespace = tagNamespace;
            _imageCleanupService = imageCleanupService;
        }

        public string[] ProcessImage(string rawDataUrl, out string intermediateUrl, out string finalUrl)
        {
            intermediateUrl = null;
            var ctx = new FCClient(_apiKey, _apiSecret);

            string contentType;
            var data = GetDataFromUrl(rawDataUrl, out contentType);
            using (var ms = new MemoryStream(data))
            {
                var detectResult = Task.Run(async () => await ctx.Faces.DetectAsync(new string[0], new[] {ms})).Result;
                ms.Seek(0, SeekOrigin.Begin);

                var image = (Bitmap)Bitmap.FromStream(ms);
                new ContrastStretch().ApplyInPlace(image);

                List<Tag> faceTags = null;
                try
                {
                    faceTags = detectResult.Photos.SelectMany(i => i.Tags).ToList();
                }
                catch (Exception ex)
                {
                    log.Error("Remote call", ex);
                }

                if (null == faceTags || faceTags.Count == 0)
                {
                    // our fancy web service can't figure out anything, use the local library :(
                    finalUrl = _imageCleanupService.CleanUpImage(rawDataUrl, out intermediateUrl);
                    return new string[0];
                }

                var intermediateImage = new Bitmap(image);
                var faces = faceTags.Select(i => GetTagBoundingBox(i, image)).ToList();
                new RectanglesMarker(faces, Color.Red).ApplyInPlace(intermediateImage);

                var boundary = Math.Max(40, faces.Max(i => Math.Max(i.Height, i.Width)) / 3);
                var x1 = Math.Max(0, faces.Min(i => i.Left) - boundary);
                var y1 = Math.Max(0, faces.Min(i => i.Top) - boundary);
                var x2 = Math.Min(image.Width, faces.Max(i => i.Right) + boundary);
                var y2 = Math.Min(image.Height, faces.Max(i => i.Bottom) + boundary);

                var newBoundingBox = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                new RectanglesMarker(new[] { newBoundingBox }, Color.Blue).ApplyInPlace(intermediateImage);

                using (var ms2 = new MemoryStream())
                {
                    intermediateImage.Save(ms2, ImageFormat.Jpeg);
                    intermediateUrl = string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(ms2.ToArray()));
                }

                image = new Crop(newBoundingBox).Apply(image);

                using (var ms2 = new MemoryStream())
                {
                    image.Save(ms2, ImageFormat.Jpeg);
                    finalUrl = string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(ms2.ToArray()));
                }

                return faceTags.Select(i => i.Matches.FirstOrDefault()).Where(i => i != null).Select(i => (i.UserId ?? "").Split('@')[0]).ToArray();
            }
            
        }

        /// <summary>
        /// Get the bounding box of the face.
        /// Center, width, and height are percent-of-image.  Convert them to absolute pixels (needed for our marker/crop algorithms)
        /// </summary>
        private Rectangle GetTagBoundingBox(Tag tag, Bitmap image)
        {
            return new Rectangle(
                (int)((tag.Center.X - tag.Width / 2) * image.Width / 100),
                (int)((tag.Center.Y - tag.Width / 2) * image.Height / 100),
                (int)((tag.Center.X + tag.Height / 2) * image.Width / 100),
                (int)((tag.Center.Y + tag.Height / 2) * image.Height / 100));
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
