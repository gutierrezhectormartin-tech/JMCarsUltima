using JMCarsWeb.DTOs;

namespace JMCarsWeb.ViewModels
{
    // La pantalla de gestion de usuarios del Administrador muestra dos tablas: clientes y escribanos
    public class UsuariosViewModel
    {
        public List<ClienteDTO> Clientes { get; set; } = new List<ClienteDTO>();

        public List<EscribanoDTO> Escribanos { get; set; } = new List<EscribanoDTO>();
    }
}
