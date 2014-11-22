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

using Neurosky;

namespace MeditatorKoala
{
   
    public sealed partial class MainPage : Page
    {
        private Koala m_koala = new Koala();
        CoreDispatcher dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;

        public MainPage()
        {
            this.InitializeComponent();

            Koala.DataContext = m_koala;

            ManagerPanel.OnNewGame += ManagerPanel_OnNewGame;
            ManagerPanel.OnConnect += ManagerPanel_OnConnect;
            ManagerPanel.OnOptions += ManagerPanel_OnOptions;
            
            OptionPanel.OnClose += OptionPanel_OnClose;

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

        public async void ManagerPanel_OnNewGame(object sender, EventArgs e)
        {
            if (Neurosky.Mindwave.Device.IsConnected)
            {
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {                   
                    StopWatchIndicator.StopWatch.Reset();
                    StopWatchIndicator.StopWatch.Start();
                    ManagerPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //IsGameStarted = true;
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

    }
}
