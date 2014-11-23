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
            Awake,
            Hyperactivity,
            Fallen
        };

        private States m_state;
        public States State {
            get
            {
                return m_state;
            }

            set
            {
                m_state = value;
                switch (m_state)
                {
                    case States.Sleeping: CurrentFrameIndex = 0; break;
                    case States.Awake: CurrentFrameIndex = 7; break;
                    case States.Hyperactivity: CurrentFrameIndex = 13; break;
                    default: CurrentFrameIndex = 0; break;
                }
                NotifyPropertyChanged();
            }
        }


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

        public static int FrameCount = 40;
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
                            if (CurrentFrameIndex > 6)
                            {
                                CurrentFrameIndex = 0;
                            }
                            CurrentFrame = Frame[CurrentFrameIndex];
                            break;
                        }
                    case States.Awake:
                        {
                            CurrentFrameIndex++;                                                   
                            if (CurrentFrameIndex > 13)
                            {
                                CurrentFrameIndex = 7;
                            }
                            CurrentFrame = Frame[CurrentFrameIndex];
                            break;
                        }
                    case States.Hyperactivity:
                        {
                            CurrentFrameIndex++;
                            if (CurrentFrameIndex > 39)
                            {
                                CurrentFrameIndex = 39;
                                State = States.Fallen;
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
