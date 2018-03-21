using System;
using System.Collections.Generic;
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

// Librerias en remote-wiring-experience
using System.Text;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Notifications;
using Microsoft.Maker.RemoteWiring;
using System.Diagnostics;
using Windows.UI.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using HogaTron.Model;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace HogaTron
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class PageRiego : Page
    {
        //private RemoteDevice arduino;
        private string _zona;
        private int _decMinutos, _uniMinutos,_decSegundos, _uniSegundos;
        private byte IOpinBomba = 0, IOpinValvula1 = 0, IOpinValvula2 = 0;
        //private Timer timerRiego;
        DispatcherTimer timerRiego = new DispatcherTimer();
        //public event EventHandler Resize;

        public PageRiego()
        {
            this.InitializeComponent();            
            timerRiego.Interval = new TimeSpan(0, 0, 1);
            timerRiego.Tick += TimerRiego_Tick;
            PageRiego_cs.SizeChanged += PageRiego_Resize;
            BarRiego.Width = Window.Current.Bounds.Width - 150;
            
            if (App.arduinoConectado)
            {
                UpdateUI();
                if (BtnRegar.IsChecked == false)     // despues de actualizar UI, si el boton no esta presionado entonces no se esta regando
                {
                    InputOutput.ConfiguarControlador("Arduino riego");
                }
                else
                {
                    App.pageResuming = true;
                }
        }

        private void PageRiego_Resize(object sender, SizeChangedEventArgs e)
        {
            BarRiego.Width = Window.Current.Bounds.Width - 150;
        }

        protected override void OnNavigatedTo( NavigationEventArgs e )
        {
            base.OnNavigatedTo( e );
            if (App.pageResuming)   // comparar la hora de inicio del timer con la hora de retomar la app luego de suspender
            {
                BarRiego.Maximum = GetTimeTotal();
                BarRiego.Value = GetTimeLeft();
                TxtRiego.Text = BarRiego.Value.ToString();

                App.appResuming = App.pageResuming = false;     // se resetea appResuming porque se suspendio en esta pagina
                timerRiego.Start();
            }
        }

        private void Zona_Click(object sender, RoutedEventArgs e)
        {
            var selected = (MenuFlyoutItem)sender;
            _zona = selected.Text;
            SelZona.Content = selected.Text;
        }

        private void DecMinutos_Click(object sender, RoutedEventArgs e)
        {
            var selected = (MenuFlyoutItem)sender;
            _decMinutos = Convert.ToInt16(selected.Text);
            SelDecMinutos.Content = selected.Text;
        }

        private void UniMinutos_Click(object sender, RoutedEventArgs e)
        {
            var selected = (MenuFlyoutItem)sender;
            _uniMinutos = Convert.ToInt16(selected.Text);
            SelUniMinutos.Content = selected.Text;
        }

        private void DecSegundos_Click(object sender, RoutedEventArgs e)
        {
            var selected = (MenuFlyoutItem)sender;
            _decSegundos = Convert.ToInt16(selected.Text);
            SelDecSegundos.Content = selected.Text;
        }

        private void UniSegundos_Click(object sender, RoutedEventArgs e)
        {
            var selected = (MenuFlyoutItem)sender;
            _uniSegundos = Convert.ToInt16(selected.Text);
            SelUniSegundos.Content = selected.Text;
        }

        public void FuncionRiego(bool comenzar)
        {
            //var displayRequest = new Windows.System.Display.DisplayRequest();  //para evitar que se apague la pantalla

            if (comenzar)
            {
                if (_zona == "Ninguna" ||  String.IsNullOrEmpty(_zona))
                {
                    MessageBox("Seleccione la zona a regar");
                    BtnRegar.IsChecked = false;
                }









                
                else
                {
                    //displayRequest.RequestActive(); //to request keep display on
                    App.Arduino.digitalWrite(IOpinValvula, PinState.HIGH);
                    TxtAccion.Text = "Regando " + _zona;
                    App.Arduino.digitalWrite(IOpinBomba, PinState.HIGH);
                }
            }
            else
            {
                App.Arduino.digitalWrite(IOpinValvula, PinState.LOW);
                TxtAccion.Text = "No se está regando ";
                App.Arduino.digitalWrite(IOpinBomba, PinState.LOW);
                //displayRequest.RequestRelease(); //to release request of keep display on
            }

        }

        public async void MessageBox(string mensaje)
        {
                    MessageDialog showDialog = new MessageDialog(mensaje);
                    showDialog.Commands.Add(new UICommand("OK")
                    {
                        Id = 0
                    });
                    showDialog.DefaultCommandIndex = 1;
                    var result = await showDialog.ShowAsync();            
        }

        private void BtnRegar_Click(object sender, RoutedEventArgs e)
        {
            if (BtnRegar.IsChecked == true)
            {
                //TimeSpan tiempoRiego = new TimeSpan(0, (_decMinutos * 10) + (_uniMinutos), (_decSegundos * 10) + (_uniSegundos));
                App.riegoManual.SpanEvento = new TimeSpan(0, (_decMinutos * 10) + (_uniMinutos), (_decSegundos * 10) + (_uniSegundos));
                App.riegoManual.HoraEvento = DateTime.Now.Add(App.riegoManual.SpanEvento);
                //App.timeLeft = (_decMinutos * 10 * 60) + (_uniMinutos * 60) + (_decSegundos * 10) + (_uniSegundos);
                //TimeSpan timeLeftSpan = FromSeconds(App.riegoManual.horaEvento.Subtract(App.riegoManual.spanEvento));
                //int timeLeftInt = int.Parse(FromSeconds(timeLeftSpan.ToString));
                BarRiego.Maximum = GetTimeTotal();
                BarRiego.Value = BarRiego.Maximum;
                TxtRiego.Text = BarRiego.Maximum.ToString();
                timerRiego.Start();
                if (App.arduinoConectado)
                {                    
                    FuncionRiego(true);
                }
            }
            else
            {
                TxtRiego.Text = "0";
                BarRiego.Value = 0;
                timerRiego.Stop();
                if (App.arduinoConectado)
                {
                    FuncionRiego(false);
                }
            }
            if (App.arduinoConectado) UpdateUI();

        }        

        private void TimerRiego_Tick(object sender, object e)
        {
            if (App.appResuming)
            {
                BarRiego.Value = GetTimeLeft();
            }

            if (BarRiego.Value > 0)
            {
                BarRiego.Value--;
                TxtRiego.Text = BarRiego.Value.ToString();
            }
            else
            {
                timerRiego.Stop();
                BtnRegar.IsChecked = false;
                if (App.arduinoConectado)
                    FuncionRiego(false);
                TxtRiego.Text = "0";
                BarRiego.Value = 0;
            }            
        }

        private double GetTimeTotal ()
        {
            //TimeSpan timeLeftSpan = App.riegoManual.horaEvento.Subtract(App.riegoManual.spanEvento);
            //int timeLeftInt = int.Parse(FromSeconds(timeLeftSpan.ToString));
            TimeSpan timeLeftSpan = App.riegoManual.SpanEvento;
            //double timeLeftInt = TimeSpan.FromSeconds(timeLeftSpan);
            return timeLeftSpan.TotalSeconds;
        }

        private double GetTimeLeft ()
        {
            TimeSpan timeLeftSpan = App.riegoManual.HoraEvento.Subtract(DateTime.Now);
            return System.Convert.ToInt32(timeLeftSpan.TotalSeconds);
            //int timeLeftInt = int.Parse(FromSeconds(timeLeftSpan.ToString));
            //return timeLeftInt;
        }

        private void UpdateUI ()
        {
            if (App.Arduino.digitalRead(IOpinBomba) == PinState.HIGH)
            {
                BtnRegar.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                BtnRegar.Content = "Detener";
                BtnRegar.IsChecked = true;
            }
            else
            {
                BtnRegar.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                BtnRegar.Content = "Comenzar";
                BtnRegar.IsChecked = false;
                BarRiego.Value = 0;
            }

                /* if (!toggle)
        {
            x = (SolidColorBrush)backButton.Background;
            backButton.Background = new SolidColorBrush(Colors.Blue);
            toggle = true;
        }
        else
        {
            backButton.Background = x;
            toggle = false;
        }*/

        }
    }
}
