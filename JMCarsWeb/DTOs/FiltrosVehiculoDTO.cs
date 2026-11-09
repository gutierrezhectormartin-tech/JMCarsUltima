namespace JMCarsWeb.DTOs
{
    public class FiltrosVehiculoDTO
    {
        public List<ModeloDTO> Modelos { get; set; } = new List<ModeloDTO>();
        public List<int> Anios { get; set; } = new List<int>();
    }
}
