using System;
using Photo_Processing;
using NUnit.Framework;
using System.Drawing;


namespace MainWindowViewModelTests
{
    [TestFixture]
    public class UnitTest1
    {
        public Bitmap BitmapGenerator()
        {
            int width = 640, height = 320;
            Bitmap bmp = new Bitmap(width, height);
            Random rand = new Random();
            for (int PixelY = 0; PixelY < height; PixelY++)
            {
                for (int PixelX = 0; PixelX < width; PixelX++)
                {

                    int Alpha = rand.Next(256);
                    int ColorRed = rand.Next(256);
                    int ColorGreen = rand.Next(256);
                    int ColorBlue = rand.Next(256);


                    bmp.SetPixel(PixelX, PixelY, Color.FromArgb(Alpha, ColorRed, ColorGreen, ColorBlue));
                }
            }
            return bmp;
        }
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TextTimerShouldReturnString()
        {
            var viewModel = new MainWindowViewModel();

            viewModel.TextTimer = "Test";

            Assert.AreEqual("Test", viewModel.TextTimer);
        }


        [Test]
        public void OldImageShouldReturnString()
        {
            var viewModel = new MainWindowViewModel();

            viewModel.OldImage = "Test";

            Assert.AreEqual("Test", viewModel.OldImage);

        }

        [Test]
        public void LoadFromMemory_Should_BeNotNull()
        {
            
            var viewModel = new MainWindowViewModel();

            var result = viewModel.LoadFromMemory(BitmapGenerator());

            Assert.IsNotNull(result);
        }

        [Test]
        public void NewImage_Should_BeNotNull()
        {
            var viewModel = new MainWindowViewModel();

            viewModel.LoadFromMemory(BitmapGenerator());
            var result = viewModel.NewImage;

            Assert.IsNotNull(result);
        }
    }
}
