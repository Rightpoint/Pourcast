using System;
using System.Drawing.Imaging;
using System.IO;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    internal static class ImageHelper
    {
        /// <summary>
        /// Create an ImageCodecInfo for ImageFormat.Jpeg
        /// </summary>
        public static ImageCodecInfo JPEGEncoder()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// Create an EncoderParameters with the given quality level
        /// </summary>
        public static EncoderParameters Quality(long value)
        {
            var p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.Quality, value);
            return p;
        }

        /// <summary>
        /// Get the bytes written to the stream passed to the callback
        /// </summary>
        public static byte[] GetBytes(Action<Stream> callback)
        {
            using (var ms = new MemoryStream())
            {
                callback(ms);
                return ms.ToArray();
            }
        }

    }
}