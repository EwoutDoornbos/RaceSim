using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Common;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Media.Media3D;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.IO;

namespace WpfProject
{

    public static class ImagesCache
    {
        private static Dictionary<string, Bitmap> imagesCache;
        public static void Initialize()
        {
            imagesCache = new Dictionary<string, Bitmap>();
        }
        public static Bitmap GetImageBitmap(string URL)
        {
            Bitmap bitmapClone;
            Bitmap bitmap;
            try
            {
                bitmap = (Bitmap)imagesCache[URL];
                lock (new object())
                {
                    bitmapClone = (Bitmap)bitmap.Clone();
                }
                return bitmapClone;
            }
            catch (KeyNotFoundException)
            {
                bitmap = new(URL);
                imagesCache.Add(URL, bitmap);
                lock (new object())
                {
                    bitmapClone = (Bitmap)bitmap.Clone();
                }
                return bitmapClone;
            }
        }
        public static void clearCache()
        {
            imagesCache.Clear();
        }
        public static Bitmap GetBitmapEmpty(int dimensionX, int dimensionY)
        {

            try
            {
                return (Bitmap)imagesCache["empty"].Clone();
            }
            catch (KeyNotFoundException)
            {
                Bitmap bitmap = new Bitmap(dimensionX, dimensionY);
                Graphics g = Graphics.FromImage(bitmap);
                g.FillRectangle(new SolidBrush(System.Drawing.Color.LightGreen), 0, 0, dimensionX, dimensionY);
                imagesCache["empty"] = bitmap;
                return (Bitmap)bitmap.Clone();
            }
        }
        public static BitmapSource GetBitmapSource(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

    }
}