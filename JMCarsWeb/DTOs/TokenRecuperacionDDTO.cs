namespace JMCarsWeb.DTOs
{
    public class TokenRecuperacionDDTO
    {
        public int IdToken { get; set; }
        public int IdUsuario { get; set; }
        public string Token { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; }

    }
}
