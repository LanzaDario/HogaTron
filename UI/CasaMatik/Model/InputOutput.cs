using System;
using Windows.Devices.Gpio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogaTron.Model
{        
    class InputOutput    
    {
    /// <summary>
    /// Se llama para configurar un controlador que se quiere utilizar. Esto va a establecer la conexion si no esta conectado,
    /// buscar en la tabla de configuraciones todos los elementos conectados a ese controlador, configurar sus pines como
    /// entrada o salida, etc.
    /// </summary>
        public ConfiguarControlador(string controlador)
        {
            int controlador_ID = TablasAjustes.controladores.Where(p => p.Descripcion == controlador).First().IDCont;

            var dispositivos = from instrumento in TablasAjustes.entradassalidas
                               where instrumento.IDCont == controlador_ID
                               select new { instrumento.PinNum, instrumento.TipoIO };
            foreach (var instrumento in dispositivos)
            {
                if (controlador == "Raspberry PI")       // El controlador es el Raspberry PI donde corre la app
                {
                    CtrlRaspberryPI.IOInicializar(instrumento.PinNum, instrumento.TipoIO);
                }
                else    // El controlador es un Arduino remoto
                {
                    CtrlArduino.IOInicializar(instrumento.PinNum, instrumento.TipoIO);
                }
            }
        }

        public Main(string[] args)
        {
            InputOutput.CargarConfiguracion();

            Console.WriteLine("Indique la tarea de control que quiere realizar\n");
            foreach (TareaControl tarea in Configuraciones.tareacontrol)
            {
                Console.WriteLine("{0}: {1}", tarea.IDtarea, tarea.Descripcion);
            }

            Console.WriteLine("\nOpción: ");
            int Respuesta = int.Parse(Console.ReadLine());
            string tareaSeleccionada = Configuraciones.tareacontrol.Where(p => p.IDtarea == Respuesta).First().Descripcion;
            Console.WriteLine("\nLa tarea '{0}' consiste en:\n", tareaSeleccionada);

            var accionesControl = from accion in Configuraciones.accioncontrol
                       where accion.IDtarea == Respuesta
                       select new { accion.Tag, accion.EstadoIO };
            
            int i=0;
            foreach (var accion in accionesControl)
            {
                i++;
                string estado = accion.EstadoIO == "PinState.HIGH"? "Encender" : "Apagar";
                string descripcionIO = Configuraciones.entradassalidas.Where(p => p.Tag == accion.Tag).First().Descripcion;
                Console.WriteLine("{0}º: {1} la {2} {3}", i, estado, descripcionIO, accion.Tag);
            }
        }
    }
}