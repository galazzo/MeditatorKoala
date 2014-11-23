using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

namespace MeditatorKoala
{
    public sealed partial class Display : UserControl
    {
        public string IconSource
        {            
            set
            {
                Icon.Source = new BitmapImage(new Uri(value));
            }
        }

        public string TextValue
        {
            get
            {
                return Text.Text;
            }

            set
            {
                Text.Text = value;
            }
        }

        public bool ShowProgressBar
        {
            get
            {
                return PbarConsole.Visibility == Windows.UI.Xaml.Visibility.Visible;
            }

            set
            {
                if (value == true)
                {
                    PbarConsole.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    Text.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    RaceStopWatch.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    PbarConsole.Visibility = Windows.UI.Xaml.Visibility.Collapsed;                    
                }
            }
        }

        public bool ShowStopWatch
        {
            get
            {
                return RaceStopWatch.Visibility == Windows.UI.Xaml.Visibility.Visible;
            }

            set
            {
                if (value == true)
                {
                    PbarConsole.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Text.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    RaceStopWatch.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    RaceStopWatch.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }

        public StopWatchControl StopWatch
        {
            get
            {
                return RaceStopWatch;
            }
        }

        public double ProgressBar
        {
            get
            {
                return Pbar.Value;
            }

            set
            {
                Pbar.Value = value;
            }
        }

        public Display()
        {
            this.InitializeComponent();
        }

        private void Pbar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if( sender != null)
            {
                PbarValue.Text = (sender as ProgressBar).Value.ToString();
            }
        }

        /*private async void SetImageSource(string fileName)
        {
            var file = await Windows.Storage.KnownFolders.PicturesLibrary.GetFileAsync(fileName);
            var stream = await file.OpenReadAsync();
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);

            Icon.Source = bitmapImage;
        }*/

    }
}
