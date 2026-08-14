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

        Vehiculo DetalleVehiculo(int IdVehiculo);

        List<Vehiculo> BuscarGeneral(decimal pLatCli, decimal pLonCli, int pRadioKm, int? pIdMarca, decimal? pPrecioMax);

        void Registrar(Vehiculo pVehiculo);

        void Modificar(Vehiculo pVehiculo);

        void Inactivar(int pIdVehiculo);

        void Activar(int pIdVehiculo);
    }
}
