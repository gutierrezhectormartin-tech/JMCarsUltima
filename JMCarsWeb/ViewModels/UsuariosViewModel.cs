using JMCarsWeb.DTOs;
using X.PagedList;
using X.PagedList.Extensions;

namespace JMCarsWeb.ViewModels
{
    public class UsuariosViewModel
    {
        public string Tipo { get; set; }
        public string Buscar { get; set; }
        public bool? Activo { get; set; }
        public int TotalFiltrados { get; set; }

        public IPagedList<ClienteDTO> Clientes { get; set; } = new List<ClienteDTO>().ToPagedList(1, 10);
        public IPagedList<EscribanoDTO> Escribanos { get; set; } = new List<EscribanoDTO>().ToPagedList(1, 10);
    }
}
