using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogaTron.Core
{        
    public class InputOutput    
    {
    /// <summary>
    /// Se llama para configurar un controlador que se quiere utilizar. Esto va a establecer la conexion si no esta conectado,
    /// buscar en la tabla de configuraciones todos los elementos conectados a ese controlador, configurar sus pines como
    /// entrada o salida, etc.
    /// </summary>
        public static void ConfigurarControlador(string controlador)
        {
            int controlador_ID = TablasAjustes.controladores.Where(p => p.Descripcion == controlador).First().IDcont;

            var dispositivos = from instrumento in TablasAjustes.entradassalidas
                               where instrumento.IDcont  == controlador_ID
                               select new { instrumento.PinNum, instrumento.TipoIO };
            foreach (var instrumento in dispositivos)
            {
                if (controlador == "Raspberry PI")       // El controlador es el Raspberry PI donde corre la app
                {
                    //CtrlRaspberryPI.IOInicializar(instrumento.PinNum, instrumento.TipoIO);
                }
                else if (controlador.Contains("Arduino"))    // El controlador es un Arduino remoto
                {
                    //CtrlArduino.IOInicializar(instrumento.PinNum, instrumento.TipoIO);
                }
                else if (controlador == "Simulador")    // El controlador es un Arduino remoto
                {
                    //CtrlSimulator.IOInicializar(instrumento.PinNum, instrumento.TipoIO);
                }
            }
        }

    /// <summary>
    /// Se llama para ejecutar una tarea de la tabla {tareacontrol}. Filtra todas las acciones que requiere esa tarea, busca a que
    /// procesador pertenece cada instrumento y pide que se encienda o apague cada uno de los listados en la {accioncontrol}.
    /// </summary>
        public static void RunTask (int idTarea, int tiempo)
        {
            var accionesControl = from accion in TablasAjustes.accioncontrol
            where accion.IDtarea == idTarea
            select new { accion.Tag, accion.EstadoIO };
   
            int i=0;
            foreach (var accion in accionesControl)
            {
                i++;
                int idCont = TablasAjustes.entradassalidas.Where(p => p.Tag == accion.Tag).First().IDcont;
                string contr = TablasAjustes.controladores.Where(p => p.IDcont == idCont).First().Descripcion;
                string estado = accion.EstadoIO == "On"? "Encender" : "Apagar";
                string descripcionIO = TablasAjustes.entradassalidas.Where(p => p.Tag == accion.Tag).First().Descripcion;
                Console.WriteLine("{0}º: {1} la {2} {3} en {4}", i, estado, descripcionIO, accion.Tag, contr);
            }
        }
    }
}