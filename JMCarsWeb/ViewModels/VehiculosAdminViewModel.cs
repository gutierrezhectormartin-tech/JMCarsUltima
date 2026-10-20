using JMCarsWeb.DTOs;
using X.PagedList;

namespace JMCarsWeb.ViewModels
{
    public class VehiculosAdminViewModel
    {
        public IPagedList<VehiculoDTO> Vehiculos { get; set; }
        public List<string> Marcas { get; set; }
        public int? Estado { get; set; }
        public string? Marca { get; set; }
        public string? Buscar { get; set; }
        public int TotalFiltrados { get; set; }

    }
}
