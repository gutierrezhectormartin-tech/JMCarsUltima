using JMCarsWeb.DTOs;
using X.PagedList;
using X.PagedList.Extensions;

namespace JMCarsWeb.ViewModels
{
    public class HistorialPublicacionesViewModel
    {
        public IPagedList<VehiculoDTO> Vehiculos { get; set; } = new List<VehiculoDTO>().ToPagedList();
        public int IdCliente { get; set; }
        public string? NombreCliente { get; set; }
        public int? Estado { get; set; }
        public string? Buscar { get; set; }
        public int TotalFiltrados { get; set; }
    }
}
