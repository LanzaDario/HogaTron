using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Windows.UI.Xaml;
using Microsoft.Maker.Serial;
using Microsoft.Maker.RemoteWiring;

using System.Threading;
using Windows.UI.Popups;

namespace HogaTron.Model
{
    //public bool StatConexion { get; set; }
    //internal static bool StatConexion = false;

    class Sistema
    {
        /// <summary>
        /// Crea un mensaje emergente en pantalla con el texto y las opciones con que se llame
        /// </summary>
        enum Botones
        {
            OkOnly,
            YesNo,
            YesNoCancel
        }
        
        public async void MessageBox(string mensaje, Botones botones)
        {            
            switch (botones)
            {
                case Botones.OkOnly:
                    MessageDialog showDialog = new MessageDialog(mensaje);
                    showDialog.Commands.Add(new UICommand("OK")
                    {
                        Id = 0
                    });
                    showDialog.DefaultCommandIndex = 1;
                    var result = await showDialog.ShowAsync();
                    break;
            }
        }

        /// <summary>
        /// Crea listas para cada categoría en el archivo .csv de configuraciones. Lee el archivo línea por línea y carga un nuevo
        /// registro a la lista con cada línea del archivo. Luego se accede a esta lista para buscar y filtrar el dato necesario.
        /// </summary>
        static public void CargarAjustes()        // Opcion #2: Carga las listas con todas las configuraciones del archivo5
        {
            string grupoConfig = "", registroBD = "";
            int e = 0;
            string[] campoBD = new string[10];      // TODO: Hacerlo dinámico

            bool skipNextLine = false;
            //var controladores = new List<Controlador>();
            TablasAjustes.controladores = new List<Controlador>();
            TablasAjustes.entradassalidas = new List<EntradaSalida>();
            TablasAjustes.tareacontrol = new List<TareaControl>();
            TablasAjustes.accioncontrol = new List<AccionControl>();
            TablasAjustes.calendario = new List<Calendario>();

            foreach (string line in File.ReadLines(@"Model\HogaTron.cfg"))
            {
                registroBD = line;

                if (line.Contains("{"))
                {
                    grupoConfig = registroBD.Substring(1, registroBD.Length - 2);
                    skipNextLine = true;                // para saltear la línea siguiente al grupoConfig que son los campos de la tabla
                    continue;
                }
                else if (line == "") continue;

                if (!skipNextLine)
                {
                    do
                    {
                        campoBD[e] = registroBD.Substring(0, registroBD.IndexOf(","));
                        registroBD = registroBD.Substring(registroBD.IndexOf(",") + 2);
                        e++;
                        if (registroBD.IndexOf(",") == -1)
                            campoBD[e] = registroBD;
                    } while (registroBD.IndexOf(",") != -1);
                    e = 0;

                    switch (grupoConfig)
                    {
                        case "Controlador":
                            TablasAjustes.controladores.Add(new Controlador { IDcont = int.Parse(campoBD[0]), Descripcion = campoBD[1], IPdir = campoBD[2], Puerto = ushort.Parse(campoBD[3]) });
                            break;

                        case "EntradaSalida":
                            TablasAjustes.entradassalidas.Add(new EntradaSalida { Tag = campoBD[0], IDcont = int.Parse(campoBD[1]), TipoIO = campoBD[2], PinNum = byte.Parse(campoBD[3]), Descripcion = campoBD[4], Servicio = campoBD[5] });
                            break;

                        case "TareaControl":
                            TablasAjustes.tareacontrol.Add(new TareaControl { IDtarea = int.Parse(campoBD[0]), Categoria = campoBD[1], Descripcion = campoBD[2] });
                            break;

                        case "AccionControl":
                            TablasAjustes.accioncontrol.Add(new AccionControl { IDtarea = int.Parse(campoBD[0]), Tag = campoBD[1], EstadoIO = campoBD[2] });
                            break;

                        case "Calendario":
                            TablasAjustes.calendario.Add(new Calendario { IDtarea = int.Parse(campoBD[0]), DiaPrograma = campoBD[1], Hora = DateTime.Parse(campoBD[2]) , Tiempo = int.Parse(campoBD[3]) });
                            break;
                    }
                }
                skipNextLine = false;
            }
        }
    }
}
