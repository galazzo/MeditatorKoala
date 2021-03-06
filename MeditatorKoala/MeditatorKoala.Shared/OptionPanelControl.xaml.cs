﻿using System;
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

namespace MeditatorKoala
{
    public sealed partial class OptionPanelControl : UserControl
    {
        public event EventHandler<EventArgs> OnClose;
        
        public int Device
        {
            get
            {
                return DeviceList.SelectedIndex;
            }
        }

        public int Difficulty
        {
            get
            {
                int result = 100;
                switch (DifficultyLevel.SelectedIndex)
                {
                    case 0: result = 50; break;
                    case 1: result = 75; break;
                    case 2: result = 91; break;
                    default: result = 75; break;
                }
                return result ;
            }
        }

        public OptionPanelControl()
        {
            this.InitializeComponent();
        }

        private async void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            await Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            });

            EventHandler<EventArgs> handler = OnClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
