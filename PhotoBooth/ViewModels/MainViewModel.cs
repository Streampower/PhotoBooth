using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBooth.Model;

namespace PhotoBooth.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Random _rnd = new Random(Guid.NewGuid().GetHashCode());
        public List<Device> Devices { get; set; }
        public List<BitmapImage> Images { get; set; }
        public BitmapImage ActiveBitmapImage { get; set; }
        public RelayCommand LoadDevices { get; set; }

        public MainViewModel()
        {
            LoadDevices = new RelayCommand(() =>
            {
                GetDevices();
                LoadImages();
            });
            var interval = _rnd.Next(3, 6);
            _timer.Interval = new TimeSpan(0, 0, interval);
            _timer.Tick += (sender, o) => { ActiveBitmapImage = Images.ElementAt(_rnd.Next(0, Images.Count)); };
            _timer.Start();
        }

        private async void LoadImages()
        {
            if (Images == null)
                Images = new List<BitmapImage>();

            var pictureLibrary = KnownFolders.PicturesLibrary;
            var result = pictureLibrary.CreateFileQuery(CommonFileQuery.DefaultQuery);
            var images = await result.GetFilesAsync();
            foreach (var storageFile in images)
            {
                var stream = await storageFile.OpenReadAsync();
                var bitMap = new BitmapImage();
                bitMap.SetSource(stream);
                Images.Add(bitMap);
            }
            //Images.AddRange(images.Select(image => new BitmapImage { UriSource = new Uri(image.Path)}).ToList());
            RaisePropertyChanged(() => Images);
        }

        private void GetDevices()
        {
            if (Devices == null)
                Devices  = new List<Device>();

            var devices =  from n in Enumerable.Range(1, 6)
                select new Device
                {
                    Maker = "Maker " + n,
                    Model = "Model " + n,
                    ImageSource = (n % 2 == 0) ? new Uri("ms-appx:///Assets/android-logo.jpg")
                        : new Uri("ms-appx:///Assets/apple-logo.png")
                };

            Devices.AddRange(devices.ToList());
            RaisePropertyChanged(() => Devices);
        }
    }
}
