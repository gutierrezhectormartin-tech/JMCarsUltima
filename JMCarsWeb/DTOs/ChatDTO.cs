namespace JMCarsWeb.DTOs
{
    public class ChatDTO
    {
        public int IdChat { get; set; }
        public DateTime FechaInicio { get; set; }
        public int IdVehiculo { get; set; }
        public List<MensajeDTO> Mensajes { get; set; }
        public string NombreOtroUsuario { get; set; }
        public string UltimoMensaje { get; set; }
        public DateTime? FechaUltimoMensaje { get; set; }

    }
}
