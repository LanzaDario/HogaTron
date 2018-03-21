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
using CasaMatik.Model;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace CasaMatik
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage2 : Page
    {
        public MainPage2()
        {
            this.InitializeComponent();
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);
            //BackButton.Visibility = Visibility.Collapsed;
            //MyFrame.Navigate(typeof(PageEstado));
            //Estado.IsSelected = true;
            TitleTextBlock.Text = "Estado del Sistema";
            InputOutput.CargarConfiguracion();
            if (App.arduinoConectado)
            {
                StatConexion.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                StatConexion.Icon = "MapDrive"
            }
            else
            {
                StatConexion.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                StatConexion.Icon = "DisconnectDrive"
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {

            // you can also add items in code behind
            /*NavView.MenuItems.Add(new NavigationViewItemSeparator());
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "My content", Icon = new SymbolIcon(Symbol.Folder), Tag = "content" });*/

            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "estado")
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(PageConexion));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);

            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(PageConexion));
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                NavView_Navigate(item);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "estado":
                    ContentFrame.Navigate(typeof(PageEstado));
                    break;

                case "alarma":
                    ContentFrame.Navigate(typeof(PageAlarma));
                    break;

                case "riego":
                    ContentFrame.Navigate(typeof(PageRiego));
                    break;

                case "temperatura":
                    ContentFrame.Navigate(typeof(PageTemperatura));
                    break;

                case "iluminacion":
                    ContentFrame.Navigate(typeof(PageIluminacion));
                    break;

                case "temporizador":
                    ContentFrame.Navigate(typeof(PageTemporizador));
                    break;
            }
        }
    }
}
