using System;
using Windows.Devices.Gpio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HogaTron.Model;

namespace HogaTron.Model
{        
    class Arduino    
    {
        DispatcherTimer timeout;

        public void Conectar (string controlador)
        {
            string host = TablasAjustes.controladores.Where(p => p.Descripcion == controlador).First().IPdir;
            ushort port = TablasAjustes.controladores.Where(p => p.Descripcion == controlador).First().Puerto;

            App.Connection = new NetworkSerial(new Windows.Networking.HostName(host), port);
            
            App.Arduino = new RemoteDevice(App.Connection);
            App.Arduino.DeviceReady += OnDeviceReady;
            App.Arduino.DeviceConnectionFailed += OnConnectionFailed;

            //App.Connection.begin(baudRate, SerialConfig.SERIAL_8N1);

            //start a timer for connection timeout
            timeout = new DispatcherTimer();
            timeout.Interval = new TimeSpan(0, 0, 30);
            timeout.Tick += Connection_TimeOut;
            timeout.Start();
        }

        private void OnConnectionFailed(string message)
        {
            //var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            //{
                timeout.Stop();
            //    ConnectMessage.Text = "Connection attempt failed: " + message;

                Desconectar();
            //}));
        }

        private void OnDeviceReady()
        {
            //var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            //{
                timeout.Stop();
                //ConnectMessage.Text = "Successfully connected!";

                App.arduinoConectado = true;

                //MainPage.StatConexion.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                //App.statConexion = "&#xE8CE;";
            //}));
        }

        private void Connection_TimeOut(object sender, object e)
        {
            //var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            //{
                timeout.Stop();
                //ConnectMessage.Text = "Connection attempt timed out.";

                Desconectar();
            // }));
        }

        private void Desconectar()
        {
            if (App.Connection != null)
            {
                App.Connection.ConnectionEstablished -= OnDeviceReady;
                App.Connection.ConnectionFailed -= OnConnectionFailed;
                App.Connection.end();
            }

            App.Connection = null;
            App.Arduino = null;
        }

        private void IOInicializar(int pinNum, string tipoIO)
        {
            if (App.arduinoConectado)
            {
                App.Arduino.DigitalPinUpdated += CtrlArduino.IODigitalPinUpdated;   // ??? arreglar esto
                if (tipoIO == "Input")
                {
                    App.Arduino.pinMode(pinNum, PinMode.INPUT);
                }
                else
                {
                    App.Arduino.pinMode(pinNum, PinMode.OUTPUT);
                }
            }
        }

        /// <summary>
        /// This function is called when the Windows Remote Arduino library reports that an input value has changed for a digital pin.
        /// </summary>
        /// <param name="pin">The pin whose value has changed</param>
        /// <param name="state">the new state of the pin, either HIGH or LOW</param>
        private void IODigitalPinUpdated( byte pin, PinState state )
        {
            //we must dispatch the change to the UI thread to change the indicator image
            var action = Dispatcher.RunAsync( Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler( () =>
            
            {
                //var state = App.Arduino.digitalRead(pin);
                if (pin == IOpinBomba)
                {
                    //UpdateUI
                }
            } ) );
        }
    }
}