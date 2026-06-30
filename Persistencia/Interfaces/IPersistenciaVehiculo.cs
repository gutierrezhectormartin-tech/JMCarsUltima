using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces
{
    public interface IPersistenciaVehiculo
    {
        List<Vehiculo> ListarVehiculos();
        List<Vehiculo> ListarMisVehiculos(string idUsuario);

        List<Vehiculo> BuscarGeneral(decimal pLatCli, decimal pLonCli, int pRadioKm, int? pIdMarca, decimal? pPrecioMax);
    }
}
