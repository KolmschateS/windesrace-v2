using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace WPFApp
{
    public static class ImageCache
    {
        private static Dictionary<string, Bitmap> Bitmaps { get; set; }

        public static void Initialize()
        {
            Bitmaps = new Dictionary<string, Bitmap>();
        }

        public static Bitmap GetBitmap(string imagePath)
        {
            if (!Bitmaps.ContainsKey(imagePath))
                Bitmaps.Add(imagePath, new Bitmap(imagePath));
            return Bitmaps[imagePath];
        }

        public static void ClearCache()
        {
            Bitmaps.Clear();
        }

        public static Bitmap GetCanvas(int width, int height)
        {
            string key = "empty";
            if (!Bitmaps.ContainsKey(key))
            {
                Bitmap empty = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(empty);
                g.FillRectangle(
                    new SolidBrush(Color.FromArgb(0, Color.Transparent))
                    , 0, 0, width, height);
                Bitmaps.Add(key, empty);
            }
            return (Bitmap)Bitmaps[key].Clone();
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

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
