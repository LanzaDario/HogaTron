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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Win10
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);
            MyFrame.Navigate(typeof(PageEstado));
            Estado.IsSelected = true;
            TitleTextBlock.Text = "Estado del Sistema";
            Configuraciones.CargarAjustes();
            if (App.arduinoConectado)
            {
                StatConexion.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else
            {
                StatConexion.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }

        private void App_Resuming(Object sender, Object e)
        {
            //App.horaInicioResuming = DateTime.Now;
            App.appResuming = true;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.CanGoBack)
            {
                string currentPage = (MyFrame.Content.GetType()).ToString();
                MyFrame.GoBack();
                string nextPage = (MyFrame.Content.GetType()).ToString();
                if (currentPage == nextPage) MyFrame.GoBack();                  // porque hay que darle dos veces a backbutton para que vaya a la anterior
                nextPage = MyFrame.SourcePageType.ToString();
                nextPage = nextPage.Substring(nextPage.IndexOf("Page") + 4);
                switch (nextPage)
                {
                    case "Estado":
                        Estado.IsSelected = true;
                        break;
                    case "Alarma":
                        Alarma.IsSelected = true;
                        break;
                    case "Riego":
                        Riego.IsSelected = true;
                        break;
                    case "Temperatura":
                        Temperatura.IsSelected = true;
                        break;
                    case "Iluminacion":
                        Iluminacion.IsSelected = true;
                        break;
                    case "Temporizador":
                        Temporizador.IsSelected = true;
                        break;
                    case "Configuracion":
                        Configuracion.IsSelected = true;
                        break;
                }

            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Estado.IsSelected)
            {
                MyFrame.Navigate(typeof(PageEstado));
                TitleTextBlock.Text = "Estado";
                MySplitView.IsPaneOpen = false;
            }
            else if (Alarma.IsSelected)
            {
                MyFrame.Navigate(typeof(PageAlarma));
                TitleTextBlock.Text = "Alarma";
                MySplitView.IsPaneOpen = false;
            }
            else if (Riego.IsSelected)
            {
                MyFrame.Navigate(typeof(PageRiego));
                TitleTextBlock.Text = "Riego";
                MySplitView.IsPaneOpen = false;
            }
            else if (Temperatura.IsSelected)
            {
                MyFrame.Navigate(typeof(PageTemperatura));
                TitleTextBlock.Text = "Climatización";
                MySplitView.IsPaneOpen = false;
                App.statConexion = "&#xE8CD;";
            }
            else if (Iluminacion.IsSelected)
            {
                MyFrame.Navigate(typeof(PageIluminacion));
                TitleTextBlock.Text = "Iluminación";
                MySplitView.IsPaneOpen = false;
                App.statConexion = "&#xE8CE;";
            }
            else if (Temporizador.IsSelected)
            {
                MyFrame.Navigate(typeof(PageTemporizador));
                TitleTextBlock.Text = "Temporizador";
                MySplitView.IsPaneOpen = false;
            }
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            bool narrowView = false;
            if (ActualWidth <= 400) { narrowView = true; }

            if (StatusBox.Visibility == Visibility.Collapsed)
            {
                StatusBox.Visibility = Visibility.Visible;
                if (narrowView)
                {
                    TitleTextBlock.Visibility = Visibility.Collapsed;
                }
                //narrowView = false;
            }
            else
            {
                StatusBox.Visibility = Visibility.Collapsed;
                if (narrowView)
                {
                    TitleTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(PageConexion));
            TitleTextBlock.Text = "Conexión";
            MySplitView.IsPaneOpen = false;
        }
    }
}

