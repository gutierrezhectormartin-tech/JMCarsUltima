using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Interfaces
{
    public interface ILogicaEstadisticas
    {
        Estadisticas ObtenerConteos();
        List<CompraVentaPorMes> ObtenerComprasPorMes();
        List<MarcaPublicada> ObtenerMarcasMasPublicadas();
        List<ModeloVendido> ObtenerModelosVendidosPorMarca();
    }
}
