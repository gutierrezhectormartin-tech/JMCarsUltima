using JMCarsWeb.DTOs;
using X.PagedList;
using X.PagedList.Extensions;

namespace JMCarsWeb.ViewModels
{
    public class MisVehiculosViewModel
    {
        public IPagedList<VehiculoDTO> Vehiculos { get; set; } = new List<VehiculoDTO>().ToPagedList();
        public int? Estado { get; set; }
        public string? Buscar { get; set; }
        public int TotalFiltrados { get; set; }
    }
}
