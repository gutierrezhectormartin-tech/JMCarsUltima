using System.Net;

namespace JMCarsWeb.DTOs
{
    public class SolicitudNotarialDTO
    {
        public int IdSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int EstadoSolicitud { get; set; }
        public int IdUsuarioCliente { get; set; }
        public int IdVehiculo { get; set; }

    }
}
