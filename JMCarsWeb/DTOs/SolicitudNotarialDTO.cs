namespace JMCarsWeb.DTOs
{
    public class SolicitudNotarialDTO
    {
        public int IdSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int EstadoSolicitud { get; set; }
        public ClienteDTO Cliente { get; set; }
        public VehiculoDTO Vehiculo { get; set; }

    }
}
