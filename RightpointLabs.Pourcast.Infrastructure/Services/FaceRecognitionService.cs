using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Accord.Imaging.Filters;
using AForge.Imaging.Filters;
using log4net;
using RightpointLabs.Pourcast.Domain.Services;
using SkyBiometry.Client.FC;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    public class FaceRecognitionService : IFaceRecognitionService
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _tagNamespace;
        private string[] _possibleUsers;
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

            var dataToSend = ResizePictureForDetection(data);
            
            if (null == _possibleUsers)
            {
                _possibleUsers = Task.Run(async () => await ctx.Account.UsersAsync(new [] { _tagNamespace })).Result.Users[_tagNamespace].Select(i => i.Split('@')[0]).ToArray();
            }

            using (var ms = new MemoryStream(data))
            {
                var image = (Bitmap)Bitmap.FromStream(ms);
                new ContrastStretch().ApplyInPlace(image);

                List<Tag> faceTags = null;
                try
                {
                    var detectResult = Task.Run(async () =>
                    {
                        using (var msToSend = new MemoryStream(dataToSend))
                        {
                            return await ctx.Faces.RecognizeAsync(_possibleUsers, new string[0], new[] { msToSend }, _tagNamespace);
                        }
                    }).Result;
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

                var boundary = Math.Max(40, faces.Max(i => Math.Max(i.Height, i.Width)));
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

                return faceTags.Select(GetMatch).Where(i => i != null).ToArray();
            }
        }

        /// <summary>
        /// Scales down the byte size of the image by reducing dimensions and/or introducing compression.  It aims to get it under 2MB so the web service won't reject it.
        /// </summary>
        private byte[] ResizePictureForDetection(byte[] data)
        {
            const int maxLength = 2*1000*1000; // be conservative
            if (data == null || data.Length < maxLength)
                return data;

            using (var ms = new MemoryStream(data))
            {
                using (var rawImage = (Bitmap) Image.FromStream(ms))
                {
                    log.DebugFormat("Image is too big @ {0} bytes, {1}x{2}, shrinking", data.Length, rawImage.Width, rawImage.Height);

                    // too big - try some measures to downsize things
                    for (var cutSize = 1;; cutSize++)
                    {
                        // first, let's cut the image down
                        using (var smallerImage = new ResizeBilinear(rawImage.Width/cutSize, rawImage.Height/cutSize).Apply(rawImage))
                        {
                            log.DebugFormat("Sized down to {0}x{1}", smallerImage.Width, smallerImage.Height);

                            // now, see what a PNG would be...
                            var png = GetBytes(s => smallerImage.Save(s, ImageFormat.Png));
                            log.DebugFormat("PNG is {0} bytes", png.Length);
                            if (png.Length < maxLength)
                                return png;

                            // too big, try JPEG
                            var jpg = GetBytes(s => smallerImage.Save(s, ImageFormat.Jpeg));
                            log.DebugFormat("JPG is {0} bytes", jpg.Length);
                            if (jpg.Length < maxLength)
                                return jpg;

                            // still too big, guess we'll have to use a smaller image
                        }
                    }
                }
            }
        }

        private byte[] GetBytes(Action<Stream> callback)
        {
            using (var ms = new MemoryStream())
            {
                callback(ms);
                return ms.ToArray();
            }
        }



        private static string GetMatch(Tag tag)
        {
            var result = _GetMatch(tag);
            log.DebugFormat("Got tag with ({0}), but returning {1}", string.Join(", ", tag.Matches.Select(i => string.Format("{0}: {1}", i.UserId.Split('@')[0], i.Confidence))), result);
            return result;
        }

        private static string _GetMatch(Tag tag)
        {
            if (!tag.Matches.Any())
                return null;

            var best = tag.Matches.First();

            if (best.Confidence < tag.Matches.Skip(1).Max(i => i.Confidence) + 10)
                return null; // it basically has no idea

            return best.UserId.Split('@')[0];
        }


        /// <summary>
        /// Get the bounding box of the face.
        /// Center, width, and height are percent-of-image.  Convert them to absolute pixels (needed for our marker/crop algorithms)
        /// </summary>
        private Rectangle GetTagBoundingBox(Tag tag, Bitmap image)
        {
            return new Rectangle(
                (int)((tag.Center.X - tag.Width / 2) * image.Width / 100),
                (int)((tag.Center.Y - tag.Height / 2) * image.Height / 100),
                (int)(tag.Width * image.Width / 100),
                (int)(tag.Height * image.Height / 100));
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
