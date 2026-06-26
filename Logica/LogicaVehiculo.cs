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
    }
}
