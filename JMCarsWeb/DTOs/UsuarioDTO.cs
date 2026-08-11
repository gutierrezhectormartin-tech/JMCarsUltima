namespace JMCarsWeb.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set;}
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string? Contrasena { get; set; }
        public bool EstadoUsu { get; set; }
        public int RolUsu { get; set; }
        public DateTime? FechaAceptacionTerminos { get; set; }

    }
}
