using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SGDBMetadata
{
    class ImageConversion
    {
        public byte[] AsJpeg(byte[] data)
        {
            using (var inStream = new MemoryStream(data))
            using (var outStream = new MemoryStream())
            {
                var imageStream = Image.FromStream(inStream);
                imageStream.Save(outStream, ImageFormat.Jpeg);
                return outStream.ToArray();
            }
        }

        public byte[] Resize(byte[] data, int width)
        {
            using (var stream = new MemoryStream(data))
            {
                var image = Image.FromStream(stream);

                var height = (width * image.Height) / image.Width;
                var thumbnail = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

                using (var thumbnailStream = new MemoryStream())
                {
                    thumbnail.Save(thumbnailStream, ImageFormat.Jpeg);
                    return thumbnailStream.ToArray();
                }
            }
        }
    }
}
