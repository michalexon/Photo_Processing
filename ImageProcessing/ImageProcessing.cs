using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing
{
    public class ImageProcessing
    {
        public Bitmap bitMap;
        internal BitmapData bitData;

        public void ImageOpen(string imFile)
        {
            Bitmap bmp = new Bitmap(imFile);
            bitMap = bmp;
        }

        internal void BitsLocker()
        {
            Rectangle rect = new Rectangle(0, 0, bitMap.Width, bitMap.Height);
            BitmapData bmpData = bitMap.LockBits(rect, ImageLockMode.ReadWrite, bitMap.PixelFormat);
            bitData = bmpData;
        }

        internal int BytesPerPixelCalc() { return Bitmap.GetPixelFormatSize(bitMap.PixelFormat) / 8; }
        internal int HeightInPixelsCalc() { return bitData.Height; }
        internal int WidthInBytesCalc() { return bitData.Width * BytesPerPixelCalc(); }

        public void ToMainColors()
        {
            BitsLocker();
            int byteCount = bitData.Stride * bitMap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            for (int y = 0; y < HeightInPixelsCalc(); y++)
            {
                int currentLine = y * bitData.Stride;

                for (int x = 0; x < WidthInBytesCalc(); x += BytesPerPixelCalc())
                {
                    int oldBlue = pixels[currentLine + x];
                    int oldGreen = pixels[currentLine + x + 1];
                    int oldRed = pixels[currentLine + x + 2];

                    if (oldBlue > oldGreen && oldBlue > oldRed)
                    {
                        pixels[currentLine + x] = 255;
                        pixels[currentLine + x + 1] = 0;
                        pixels[currentLine + x + 2] = 0;
                    }
                    else if (oldGreen > oldBlue && oldGreen > oldRed)
                    {
                        pixels[currentLine + x] = 0;
                        pixels[currentLine + x + 1] = 255;
                        pixels[currentLine + x + 2] = 0;
                    }
                    else
                    {
                        pixels[currentLine + x] = 0;
                        pixels[currentLine + x + 1] = 0;
                        pixels[currentLine + x + 2] = 255;
                    }
                }
            }
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            bitMap.UnlockBits(bitData);
        }

        public async Task ToMainColorsAsync()
        {
            await Task.Run(() =>
            {
                BitsLocker();
                int byteCount = bitData.Stride * bitMap.Height;
                byte[] pixels = new byte[byteCount];
                IntPtr ptrFirstPixel = bitData.Scan0;
                Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

                for (int y = 0; y < HeightInPixelsCalc(); y++)
                {
                    int currentLine = y * bitData.Stride;

                    for (int x = 0; x < WidthInBytesCalc(); x = x + BytesPerPixelCalc())
                    {

                        int oldBlue = pixels[currentLine + x];
                        int oldGreen = pixels[currentLine + x + 1];
                        int oldRed = pixels[currentLine + x + 2];


                        if (oldBlue > oldGreen && oldBlue > oldRed)
                        {
                            pixels[currentLine + x] = 255;
                            pixels[currentLine + x + 1] = 0;
                            pixels[currentLine + x + 2] = 0;
                        }
                        else if (oldGreen > oldBlue && oldGreen > oldRed)
                        {
                            pixels[currentLine + x] = 0;
                            pixels[currentLine + x + 1] = 255;
                            pixels[currentLine + x + 2] = 0;
                        }
                        else
                        {
                            pixels[currentLine + x] = 0;
                            pixels[currentLine + x + 1] = 0;
                            pixels[currentLine + x + 2] = 255;
                        }
                    }
                }
                Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
                bitMap.UnlockBits(bitData);
            });
        }

        public void ImageSaver(string newName)
        {
            bitMap.UnlockBits(bitData);
            bitMap.Save(newName);
            bitMap.Dispose();
        }   
    }
}
