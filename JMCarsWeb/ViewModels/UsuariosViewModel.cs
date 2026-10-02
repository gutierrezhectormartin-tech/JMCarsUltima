using JMCarsWeb.DTOs;

namespace JMCarsWeb.ViewModels
{
    public class UsuariosViewModel
    {
        public List<ClienteDTO> Clientes { get; set; } = new List<ClienteDTO>();

        public List<EscribanoDTO> Escribanos { get; set; } = new List<EscribanoDTO>();
    }
}
