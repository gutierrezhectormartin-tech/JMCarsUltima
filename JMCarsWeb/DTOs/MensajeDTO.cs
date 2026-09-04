namespace JMCarsWeb.DTOs
{
    public class MensajeDTO
    {
        public int IdMensaje { get; set; }
        public int IdChat { get; set; }
        public int IdUsuarioEmisor { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaHora { get; set; }
        public string NombreEmisor { get; set; }

    }
}
