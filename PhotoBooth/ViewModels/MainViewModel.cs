using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBooth.Model;

namespace PhotoBooth.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public List<Device> Devices { get; set; }
        public RelayCommand LoadDevices { get; set; }

        public MainViewModel()
        {
            LoadDevices = new RelayCommand(() =>
            {
                var list = from n in Enumerable.Range(1, 10)
                           select new Device
                           {
                               Maker = "Maker " + n,
                               Model = "Model " + n,
                               ImageUri = (n % 2 == 0) ? new Uri("ms-appx:///Assets/android-logo.jpg")
                                                     : new Uri("ms-appx:///Assets/apple-logo.png")
                           };

                Devices = new List<Device>(list);
                RaisePropertyChanged(() => Devices);
            });
        }
    }
}
