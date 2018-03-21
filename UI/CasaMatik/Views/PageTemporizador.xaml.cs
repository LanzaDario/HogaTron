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
    public sealed partial class PageTemporizador : Page
    {
        private List<Controlador> Controladores;
        public PageTemporizador()
        {
            this.InitializeComponent();
        }

        private void BtnControladores_Click(object sender, RoutedEventArgs e)
        {
            //Controladores = ControladorManager.GetControladores();
            InputOutput.CargarConfiguracion();
        }
    }
}
