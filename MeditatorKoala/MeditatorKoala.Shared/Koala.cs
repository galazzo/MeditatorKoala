using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MeditatorKoala
{
    class Koala : INotifyPropertyChanged
    {
        public enum States { 

            Sleeping,
            Drowsiness,
            Awake,
            Hyperactivity
        };
        public States State { get; set; }

        private ImageSource m_currentFrame = null;
        public ImageSource CurrentFrame
        {
            get
            {
                return this.m_currentFrame;
            }

            set
            {
                if (value != this.m_currentFrame)
                {
                    this.m_currentFrame = value;
                    NotifyPropertyChanged();
                }
            }
        }
                
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private int CurrentFrameIndex = 0;

        public static int FrameCount = 27;
        private ImageSource[] Frame = new ImageSource[FrameCount];

        public event PropertyChangedEventHandler PropertyChanged;

        public Koala()
        {
            for (int i = 0; i < FrameCount; i++ )
            {
                var frameName = String.Format("Koala{0:00}.png", i);
                System.Diagnostics.Debug.WriteLine(frameName);                
                var uri = new Uri("ms-appx:///Assets/Koala/" + frameName, UriKind.RelativeOrAbsolute);
                Frame[i] = new BitmapImage(uri);             
            }

            CurrentFrame = Frame[0];
            State = States.Sleeping;

            dispatcherTimer.Tick += Update;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            dispatcherTimer.Start();
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Update(object sender, object e)
        {
            if ( sender != null )
            {
                switch (State)
                {
                    case States.Sleeping:
                        {
                            CurrentFrameIndex++;                                                   
                            if (CurrentFrameIndex > 26)
                            {
                                CurrentFrameIndex = 0;
                            }
                            CurrentFrame = Frame[CurrentFrameIndex];
                            break;
                        }
                    default:
                        {
                            CurrentFrame = Frame[CurrentFrameIndex];
                            break;
                        }
                }
            }
        }

    }
}
