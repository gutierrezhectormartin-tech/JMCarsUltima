namespace JMCarsWeb.DTOs
{
    public class VehiculoDTO
    {
        public int IdVehiculo { get; set; }
        public decimal Precio { get; set; }
        public int Km { get; set; }
        public int Anio { get; set; }
        public string CajaCambios { get; set; }
        public string Motorizacion { get; set; }
        public string Descripcion { get; set; }
        public bool Publicado { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public ModeloDTO Modelo { get; set; }
        public ClienteDTO Vendedor { get; set; }
        public List<string> Fotografia { get; set; }

    }
}
