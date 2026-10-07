namespace JMCarsWeb.DTOs
{
    public class EstadisticasDTO
    {
        public int CantPendientes { get; set; }
        public int CantAprobadas { get; set; }
        public int CantRechazadas { get; set; }
        public int CantInactivas { get; set; }

        public int CantEnTramite { get; set; }
        public int CantVendidas { get; set; }
        public int CantClientesActivos { get; set; }
        public int CantEscribanosActivos { get; set; }
        public int CantCompraVentasFinalizadas { get; set; }
        public int CantSolicitudesPendientesVencidas { get; set; }
    }
}
