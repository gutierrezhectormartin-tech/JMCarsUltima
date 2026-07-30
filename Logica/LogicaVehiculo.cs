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
    public class LogicaVehiculo : ILogicaVehiculo
    {
        private IPersistenciaVehiculo _persistenciaVehiculo;

        public LogicaVehiculo() 
        {
            _persistenciaVehiculo = FabricaPersistencia.GetInstancia().GetPersistenciaVehiculo();
        }

        public List<Vehiculo> ListarVehiculos()
        {
            return _persistenciaVehiculo.ListarVehiculos();
        }

        public List<Vehiculo> ListarMisVehiculos(string idUsuario)
        {
            return _persistenciaVehiculo.ListarMisVehiculos(idUsuario);
        }

        public List<Vehiculo> BuscarGeneral(decimal pLatCli, decimal pLonCli, int pRadioKM, int? pIdMarca, decimal? pPrecioMax)
        {
            return _persistenciaVehiculo.BuscarGeneral(pLatCli, pLonCli, pRadioKM, pIdMarca, pPrecioMax);
        }

        public void Registrar(Vehiculo pVehiculo)
        {
            if (pVehiculo == null)
            {
                throw new Exception("El vehículo no puede ser nulo.");
            }
            pVehiculo.Validar();

            try
            {
                _persistenciaVehiculo.Registrar(pVehiculo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica al registrar el vehículo: " + ex.Message);
            }
        }

        public Vehiculo DetalleVehiculo(int pIdVehiculo)
        {
            return _persistenciaVehiculo.DetalleVehiculo(pIdVehiculo);
        }
    }
}
    

