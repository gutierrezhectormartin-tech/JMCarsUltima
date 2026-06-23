using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class FabricaPersistencia
    {
        private static FabricaPersistencia _instancia;

        private FabricaPersistencia() { }

        public static FabricaPersistencia GetInstancia()
        {
            if (_instancia == null)
                _instancia = new FabricaPersistencia();

            return _instancia;
        }
        public IPersistenciaUsuario GetPersistenciaUsuario()
        {
            return new PersistenciaUsuario();
        }

        public IPersistenciaCliente GetPersistenciaCliente()
        {
            return new PersistenciaCliente();
        }

        public IPersistenciaEscribano GetPersistenciaEscribano()
        {
            return new PersistenciaEscribano();
        }

        public IPersistenciaVehiculo GetPersistenciaVehiculo()
        {
            return new PersistenciaVehiculo();
        }

        public IPersistenciaTokenRecuperacion GetPersistenciaTokenRecuperacion()
        {
            return new PersistenciaTokenRecuperacion();
        }

    }
}
