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

// Il modello di elemento per il controllo utente è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=234236

namespace MeditatorKoala
{
    public sealed partial class ManagerPanelControl : UserControl
    {
        public event EventHandler<EventArgs> OnNewGame;
        public event EventHandler<EventArgs> OnOptions;
        public event EventHandler<EventArgs> OnConnect;
        public event EventHandler<EventArgs> OnTutorial;

        public bool CanRunNewGame
        {
            get
            {
                return NewGame.IsEnabled;
            }

            set
            {
                NewGame.IsEnabled = value;
            }
        }

        public ManagerPanelControl()
        {
            this.InitializeComponent();
        }

        
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            EventHandler<EventArgs> handler = OnNewGame;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            EventHandler<EventArgs> handler = OnOptions;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            EventHandler<EventArgs> handler = OnConnect;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            EventHandler<EventArgs> handler = OnTutorial;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void NewGame_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            
        }
    }
}
