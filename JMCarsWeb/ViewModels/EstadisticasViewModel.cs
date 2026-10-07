using JMCarsWeb.DTOs;

namespace JMCarsWeb.ViewModels
{
    public class EstadisticasViewModel
    {
        public EstadisticasDTO Conteos { get; set; } = new EstadisticasDTO();
        public List<CompraVentaPorMesDTO> ComprasPorMes { get; set; } = new List<CompraVentaPorMesDTO>();
        public List<MarcaPublicadaDTO> MarcasPublicadas { get; set; } = new List<MarcaPublicadaDTO>();
        public List<ModeloVendidoDTO> ModelosVendidos { get; set; } = new List<ModeloVendidoDTO>();
    }
}
