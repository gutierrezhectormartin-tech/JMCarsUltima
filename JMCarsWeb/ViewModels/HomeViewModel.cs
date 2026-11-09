using JMCarsWeb.DTOs;
    
namespace JMCarsWeb.ViewModels
{
    public class HomeViewModel
    {
        public PaginaDTO<VehiculoDTO> Vehiculos { get; set; } = new PaginaDTO<VehiculoDTO>();
        public FiltrosVehiculoDTO Filtros { get; set; } = new FiltrosVehiculoDTO();
        public int? Marca { get; set; }
        public int? Modelo { get; set; }
        public int? Anio { get; set; }
        public string? OrdenPrecio { get; set; }

    }
}
