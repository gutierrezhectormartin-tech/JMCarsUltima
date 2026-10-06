using Logica.Interfaces;
using Modelo;
using Persistencia;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaEstadistica : ILogicaEstadisticas
    {
        private readonly IPersistenciaEstadisticas _persistenciaEstadisticas;
        public LogicaEstadistica()
        {
            _persistenciaEstadisticas = FabricaPersistencia.GetInstancia().GetPersistenciaEstadisticas();
        }

        public Estadisticas ObtenerConteo()
        {
            return _persistenciaEstadisticas.ObtenerConteos();
        }

        public List<CompraVentaPorMes> ObtenerComprasPorMes()
        {
            return _persistenciaEstadisticas.ObtenerComprasPorMes();
        }

        public List<MarcaPublicada> ObtenerMarcasMasPublicadas()
        {
            return _persistenciaEstadisticas.ObtenerMarcasMasPublicadas();
        }
        public List<ModeloVendido> ObtenerModelosVendidosPorMarca()
        {
            return _persistenciaEstadisticas.ObtenerModelosVendidosPorMarca();
        }
    }
}
