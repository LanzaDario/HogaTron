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
using Win10.Model;
using Microsoft.Maker.RemoteWiring;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Win10
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class PageIluminacion : Page
    {
        private byte IOpinLuz = 0;
        public PageIluminacion()
        {
            this.InitializeComponent();
            IOpinLuz = Configuraciones.entradassalidas.Where(p => p.Tag == "L-401").First().PinNum;

        }

        private void SwtLuzJardin_Toggled(object sender, RoutedEventArgs e)
        {
            if (App.arduinoConectado)
                App.Arduino.pinMode(IOpinLuz, PinMode.OUTPUT);
            {
                if (SwtLuzJardin.IsOn)
                {
                    App.Arduino.digitalWrite(IOpinLuz, PinState.HIGH);
                    TxtAccion.Text = "Luces encendidas";
                }
                else
                {
                    App.Arduino.digitalWrite(IOpinLuz, PinState.LOW);
                    TxtAccion.Text = "Luces apagadas";
                }
            }
        }
    }
}
