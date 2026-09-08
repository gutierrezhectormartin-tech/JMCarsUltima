namespace JMCarsWeb.DTOs
{
    public class MensajeDTO
    {
        public int IdChat { get; set; }
        public UsuarioDTO UsuarioEmisor { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaHora { get; set; }

    }
}
