using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;

namespace Persistencia.Interfaces
{
    public  interface IPersistenciaEstadisticas
    {
        Estadisticas ObtenerConteos();
        List<CompraVentaPorMes> ObtenerComprasPorMes();
        List<MarcaPublicada> ObtenerMarcasMasPublicadas();
        List<ModeloVendido> ObtenerModelosVendidosPorMarca();
    }
}
