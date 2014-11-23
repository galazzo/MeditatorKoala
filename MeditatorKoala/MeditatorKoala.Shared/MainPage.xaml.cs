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
using Windows.UI.Core;
using Windows.UI.Popups;
using System.ComponentModel;

using Neurosky;

namespace MeditatorKoala
{
   
    public sealed partial class MainPage : Page
    {
        private Koala m_koala = new Koala();
        CoreDispatcher dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
        Windows.ApplicationModel.Resources.ResourceLoader loader = new Windows.ApplicationModel.Resources.ResourceLoader();
        private int MinQuality = 20;
        private bool IsGameStarted = false;
        private int negativeScores = 0;
        private int difficulty = 50;

        public MainPage()
        {
            this.InitializeComponent();

            Koala.DataContext = m_koala;
            m_koala.PropertyChanged += m_koala_PropertyChanged;

            ManagerPanel.OnNewGame += ManagerPanel_OnNewGame;
            ManagerPanel.OnConnect += ManagerPanel_OnConnect;
            ManagerPanel.OnOptions += ManagerPanel_OnOptions;
            
            OptionPanel.OnClose += OptionPanel_OnClose;

            Neurosky.Mindwave.Device.StateChanged += Current_StateChanged;
            Neurosky.Mindwave.Device.CurrentValueChanged += Current_ValueChanged;

#if WINDOWS_PHONE_APP
            this.NavigationCacheMode = NavigationCacheMode.Required;
#endif


        }

        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
#if WINDOWS_PHONE_APP       
            Windows.UI.ViewManagement.StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            // Hide the status bar
            await statusBar.HideAsync();
#endif
        }

        async public void  m_koala_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
            if( e.PropertyName == "State")
            {
                System.Diagnostics.Debug.WriteLine(e.PropertyName);

                if( m_koala.State == MeditatorKoala.Koala.States.Fallen)
                {
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {                       
                        ManagerPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;                        
                    });
                }
            }
        }

        public async void ManagerPanel_OnNewGame(object sender, EventArgs e)
        {
            
            if (Neurosky.Mindwave.Device.IsConnected)
            {
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    difficulty = OptionPanel.Difficulty;
                    StopWatchIndicator.StopWatch.Reset();
                    StopWatchIndicator.StopWatch.Start();
                    ManagerPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    m_koala.State = MeditatorKoala.Koala.States.Sleeping;                    
                    negativeScores = 0;
                    IsGameStarted = true;
                });
            }
            else
            {

                var dialog = new MessageDialog("Neurosky's EEG headset is not connected!\nPlease connect device before starting the game."); await dialog.ShowAsync();
            }
        }

        public void ManagerPanel_OnConnect(object sender, EventArgs e)
        {
            if (!Neurosky.Mindwave.Device.IsConnected)
            {                
                Neurosky.Mindwave.Device.Start((OptionPanel.Device == 0) ? Neurosky.MindwaveProtocol.Mobile : Neurosky.MindwaveProtocol.ThinkGear, (OptionPanel.Device == 2));
            }
        }

        public async void ManagerPanel_OnOptions(object sender, EventArgs e)
        {
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                StopWatchIndicator.StopWatch.Stop();
                ManagerPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                OptionPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            });
        }

        public async void OptionPanel_OnClose(object sender, EventArgs e)
        {
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                ManagerPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                OptionPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            });
        }

        void Current_StateChanged(object sender, MindwaveStateChangedEventArgs e)
        {
            ManagerPanel.CanRunNewGame = (e.CurrentState == MindwaveServiceState.ConnectedWithData) ? true : false;

            switch (e.CurrentState)
            {
                case MindwaveServiceState.NotConnected: ConnectionState.TextValue = loader.GetString("ConnectionState_NotConnected"); break;
                case MindwaveServiceState.Connecting: ConnectionState.TextValue = loader.GetString("ConnectionState_Connecting"); break;
                case MindwaveServiceState.ConnectedWithNoDataYet: ConnectionState.TextValue = loader.GetString("ConnectionState_ConnectedWithNoDataYet"); break;
                case MindwaveServiceState.ConnectedWithUnreliableData: ConnectionState.TextValue = loader.GetString("ConnectionState_ConnectedWithUnreliableData"); break;
                case MindwaveServiceState.ConnectedWithData: ConnectionState.TextValue = loader.GetString("ConnectionState_ConnectedWithData"); break;
                case MindwaveServiceState.FailedConnection: ConnectionState.TextValue = loader.GetString("ConnectionState_FailedConnection"); break;
                case MindwaveServiceState.FailedDuringExecution: ConnectionState.TextValue = loader.GetString("ConnectionState_FailedDuringExecution"); break;
                case MindwaveServiceState.DeviceNotFound:
                    {
                        ConnectionState.TextValue = loader.GetString("ConnectionState_NotFound");                        
                        break;
                    }
                default: break;
            }
        }

        async void Current_ValueChanged(object sender, MindwaveReadingEventArgs e)
        { 
            if (sender != null)
            {
                var data = e.SensorReading;
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    AttentionIndicator.ProgressBar = data.eSenseMeditation;
                });

                if ( StopWatchIndicator.StopWatch.ElapsedMilliseconds > 50000) // if time left since start is more then 50 seconds
                {
                    StopGame();
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        ManagerPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    });
                }

                if ( (IsGameStarted == true) && (data.Quality <= MinQuality) )
                {
                    if (data.eSenseMeditation <= difficulty)
                    {
                        m_koala.State = MeditatorKoala.Koala.States.Awake;
                        negativeScores++;
                        
                        if( negativeScores > 20 )
                        {
                            StopGame();
                            var gap = (int)((double)(50000 - StopWatchIndicator.StopWatch.ElapsedMilliseconds)/1000.0);
                            negativeScores *= gap;
                            m_koala.State = MeditatorKoala.Koala.States.Hyperactivity;
                        }

                        NegativeScore.TextValue = String.Format("Negative Scores: {0}", negativeScores);

                    } else                         
                        {
                            m_koala.State = MeditatorKoala.Koala.States.Sleeping;
                        }
                }

            }
        }

        async private void StopGame()
        {
            if (Neurosky.Mindwave.Device.IsConnected)
            {
                Neurosky.Mindwave.Device.Stop();
            }

            IsGameStarted = false;

            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                StopWatchIndicator.StopWatch.Stop();               
                AttentionIndicator.ProgressBar = 0;
            });
        }
    }
}
