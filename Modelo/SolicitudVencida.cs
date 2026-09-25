using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public  class SolicitudVencida
    {
        public int IdSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NombreEscribano { get; set; }
        public string NombreCliente { get; set; }
        public string NombreMarca { get; set; }
        public string NombreModelo { get; set; }

    }
}
