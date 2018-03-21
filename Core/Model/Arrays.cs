using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogaTron.Core
{
    public class TablasAjustes
    {
        public static List<Controlador> controladores;
        public static List<EntradaSalida> entradassalidas;
        public static List<TareaControl> tareacontrol;
        public static List<AccionControl> accioncontrol;
        public static List<Calendario> calendario;
    }

    public class Controlador
    {
        public int IDcont { get; set; }
        public string Descripcion { get; set; }
        public string IPdir { get; set; }
        public ushort Puerto { get; set; }
    }

    public class EntradaSalida
    {
        public string Tag { get; set; }
        public int IDcont { get; set; }
        //public App.Arduino.PinMode TipoIO { get; set; }
        public string TipoIO { get; set; }
        public byte PinNum { get; set; }
        public string Descripcion { get; set; }
        public string Servicio { get; set; }
    }

    public class TareaControl
    {
        public int IDtarea { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }        
    }

    public class AccionControl
    {
        public int IDtarea { get; set; }
        public string Tag { get; set; }
        //public Arduino.PinState EstadoIO { get; set; }
        public string EstadoIO { get; set; }
    }

    public class Calendario
    {
        public int IDtarea { get; set; }
        //public DiaSemana DiaPrograma { get; set; }
        public String DiaPrograma { get; set; }
        public DateTime Hora { get; set; }
        public int Tiempo { get; set; }
    }

    public enum DiaSemana
    {
        Lunes,
        Martes,
        Miercoles,
        Jueves,
        Viernes,
        Sabado,
        Domingo,
    }

    public class EventoTiempo
    {
        public DateTime HoraEvento { get; set; }
        public TimeSpan SpanEvento { get; set; }
    }
}