using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HogaTron.Core;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int error = SystemConfig.CargarAjustes();
            if (error != 1) return;

            System.Console.WriteLine("Indique la tarea de control que quiere realizar\n");
            foreach (TareaControl tarea in TablasAjustes.tareacontrol)
            {
                System.Console.WriteLine("{0}: {1}", tarea.IDtarea, tarea.Descripcion);
            }

            System.Console.WriteLine("\nOpción: ");
            int Respuesta = int.Parse(System.Console.ReadLine());
            string tareaSeleccionada = TablasAjustes.tareacontrol.Where(p => p.IDtarea == Respuesta).First().Descripcion;
            System.Console.WriteLine("\nLa tarea '{0}' consiste en:\n", tareaSeleccionada);

            InputOutput.RunTask(Respuesta, 180);

            
/*            var accionesControl = from accion in TablasAjustes.accioncontrol
                       where accion.IDtarea == Respuesta
                       select new { accion.Tag, accion.EstadoIO };
            
            int i=0;
            foreach (var accion in accionesControl)
            {
                i++;
                string estado = accion.EstadoIO == "On"? "Encender" : "Apagar";
                string descripcionIO = TablasAjustes.entradassalidas.Where(p => p.Tag == accion.Tag).First().Descripcion;
                Console.WriteLine("{0}º: {1} la {2} {3}", i, estado, descripcionIO, accion.Tag);
            }*/
        }
    }
}