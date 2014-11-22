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

using System.Diagnostics;
using Windows.UI.Core;

namespace MeditatorKoala
{
    public sealed partial class StopWatchControl : UserControl
    {
        private Stopwatch stopWatch = new Stopwatch();
        //private CoreDispatcher dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
        private DispatcherTimer dispatcherTimer { get; set; }
                
        public bool IsRunning
        {
            get
            {
                return (stopWatch != null ) ?  stopWatch.IsRunning : false;
            }
        }

        public long ElapsedTicks
        {
            get
            {
                return (stopWatch != null ) ? stopWatch.ElapsedTicks : -1;
            }
        }

        public long ElapsedMilliseconds
        {
            get
            {
                return (stopWatch != null ) ? stopWatch.ElapsedMilliseconds : -1;
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return (stopWatch != null ) ? stopWatch.Elapsed : new TimeSpan(0);
            }
        }
        
        public StopWatchControl()
        {
            this.InitializeComponent();
            
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        public void Start()
        {
            this.stopWatch.Start();
            this.dispatcherTimer.Start();
        }

        public void Stop()
        {
            this.stopWatch.Stop();
            this.dispatcherTimer.Stop();
        }

        public void Reset()
        {
            this.stopWatch.Reset();
            this.dispatcherTimer.Stop();
        }

        public void Restart()
        {
            this.stopWatch.Restart();
            this.dispatcherTimer.Start();
        }

        public string Milliseconds
        {
            get
            {
                return MillisecondsValue.Text;
            }
        }
        public string Seconds
        {
            get
            {
                return SecondsValue.Text;
            }
        }

        public string Minutes
        {
            get
            {
                return MinutesValue.Text;
            }
        }

        public string Hours
        {
            get
            {
                return HoursValue.Text;
            }
        }

        async void dispatcherTimer_Tick(object sender, object e)
        {
            if ( (sender != null) && IsRunning)
            {
                var time = stopWatch.Elapsed;

                await Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MillisecondsValue.Text = time.ToString("fff");
                    SecondsValue.Text = time.ToString("ss");
                    MinutesValue.Text = time.ToString("mm");
                    HoursValue.Text = time.ToString("hh");
                });
            }          
        }
    }
}
