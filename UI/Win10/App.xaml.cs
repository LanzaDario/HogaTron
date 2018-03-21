using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Win10
{
    /// <summary>
    /// Proporciona un comportamiento específico de la aplicación para complementar la clase Application predeterminada.
    /// </summary>
    /// 


    sealed partial class App : Application
    {
        internal static bool arduinoConectado = false;
        internal static string statConexion = "D";      // para iconos de estado
        //internal static DateTime horaInicioResuming = DateTime.Now;    // hace falta? o con comparar ultima hh guardada con now es suficiente?
        internal static EventoTiempo riegoManual = new EventoTiempo(); // EventoTiempo riegoManual;

        internal static bool appResuming = false;
        internal static bool pageResuming = false;

        public static IStream Connection
        {
            get;
            set;
        }

        public static RemoteDevice Arduino
        {
            get;
            set;
        }

        /*public static TelemetryClient Telemetry
        {
            get;
            private set;
        }*/
        /// <summary>
        /// Inicializa el objeto de aplicación Singleton. Esta es la primera línea de código creado
        /// eje-
        /// cutado y, como tal, es el equivalente lógico de main() o WinMain().
        /// </summary>
        public App()
        {

            /*WindowsAppInitializer.InitializeAsync()
                .ContinueWith(task =>
                {
                    TelemetryConfiguration.Active.TelemetryInitializers.Add(new UwpDeviceTelemetryInitializer());
                })
                .ContinueWith(task => { Telemetry = new TelemetryClient(); });

            Telemetry = new TelemetryClient();*/

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }


        /// <summary>
        /// Se invoca cuando el usuario final inicia la aplicación normalmente. Se usarán otros puntos
        /// de entrada cuando la aplicación se inicie para abrir un archivo específico, por ejemplo.
        /// </summary>
        /// <param name="e">Información detallada acerca de la solicitud y el proceso de inicio.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // En Windows Mobile. Para no ver barra blanca en el lugar de la barra de estado
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().Hide‌​Async();
                //var statusBar = StatusBar.GetForCurrentView();
                //await statusBar.HideAsync();    
            }

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                //Telemetry.TrackEvent("Launch");

                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Se invoca cuando la aplicación la inicia normalmente el usuario final. Se usarán otros puntos
        /// </summary>
        /// <param name="sender">Marco que produjo el error de navegación</param>
        /// <param name="e">Detalles sobre el error de navegación</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            var ex = new Exception("Failed to load Page " + e.SourcePageType.FullName);
            //Telemetry.TrackException(ex);
            throw ex;
        }

        /// <summary>
        /// Se invoca al suspender la ejecución de la aplicación. El estado de la aplicación se guarda
        /// sin saber si la aplicación se terminará o se reanudará con el contenido
        /// de la memoria aún intacto.
        /// </summary>
        /// <param name="sender">Origen de la solicitud de suspensión.</param>
        /// <param name="e">Detalles sobre la solicitud de suspensión.</param>


        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Guardar el estado de la aplicación y detener toda actividad en segundo plano
            deferral.Complete();
        }
    }
}
