using System;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows;

namespace Photo_Processing
{

    public class MainWindowViewModel : BindableBase
    {
        public ICommand ButtonLoader { get; set; }
        public ICommand ButtonSync { get; set; }
        public ICommand ButtonAsync { get; set; }
        private string _OldImage { get; set; }
        public string OldImage
        {
            get { return _OldImage; }
            set { _OldImage = value; RaisePropertyChanged(nameof(OldImage)); }
        }
        private BitmapImage _NewImage { get; set; }
        public BitmapImage NewImage
        {
            get { return _NewImage; }
            set { _NewImage = value; RaisePropertyChanged(nameof(NewImage)); }
        }
        private string _TextTimer { get; set; }
        public string TextTimer
        {
            
            get { return _TextTimer; }
            set { _TextTimer=value; RaisePropertyChanged(nameof(TextTimer)); }
        }
        public MainWindowViewModel()
        {
            ButtonLoader = new DelegateCommand(ClickButtonLoader);
            ButtonSync = new DelegateCommand(ClickButtonSync);
            ButtonAsync = new DelegateCommand(ClickButtonAsync);
        }
        public BitmapImage LoadFromMemory(Bitmap bitmap)
        {

            using (MemoryStream memory = new MemoryStream())
            {
                
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return NewImage = bitmapImage;
                
            }
        }
        private void TimeShow(long Time)
        {
            TextTimer = Convert.ToString(Time);
            TextTimer = "Processed using Async method in " + TextTimer + "ms";
        }

        private void ClickButtonLoader()
        {
           
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP; *.JPG; *.PNG)| *.bmp; *.jpg; *.png;",
                RestoreDirectory = true
            };
            openFileDialog.ShowDialog();
             
            OldImage = openFileDialog.FileName;

        }
        private void ClickButtonSync()
        {
            try
            {
                ImageProcessing.ImageProcessing ip = new ImageProcessing.ImageProcessing();
                Stopwatch Timer = new Stopwatch();
                ip.ImageOpen(_OldImage);
                Timer.Start();
                ip.ToMainColors();
                Timer.Stop();
                TimeShow(Timer.ElapsedMilliseconds);
                LoadFromMemory(ip.bitMap);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("You must load a image first");
            }
            catch
            {
                MessageBox.Show("There was a problem with render picture");
            }
            
        }
        private async void ClickButtonAsync()
        {
            try
            {
                ImageProcessing.ImageProcessing ip = new ImageProcessing.ImageProcessing();
                ip.ImageOpen(_OldImage);
                Stopwatch Timer = new Stopwatch();
                Timer.Start();
                await ip.ToMainColorsAsync();
                Timer.Stop();
                TimeShow(Timer.ElapsedMilliseconds);
                LoadFromMemory(ip.bitMap);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("You must load a image first");
            }
            catch
            {
                MessageBox.Show("There was a problem with render picture");
            }

        }
    }
}
