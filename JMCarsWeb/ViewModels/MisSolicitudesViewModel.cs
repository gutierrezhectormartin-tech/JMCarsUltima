using JMCarsWeb.DTOs;
using X.PagedList;
using X.PagedList.Extensions;

namespace JMCarsWeb.ViewModels
{
    public class MisSolicitudesViewModel
    {
        public IPagedList<SolicitudEscribanoDTO> Solicitudes { get; set; } = new List<SolicitudEscribanoDTO>().ToPagedList();
        public int? Estado { get; set; }
        public string? Buscar { get; set; }
        public int TotalFiltrados { get; set; }
    }
}
