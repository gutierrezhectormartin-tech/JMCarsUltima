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

        // se llama una unica vez desde el arranque de WebApi
        // para pasarle a la persistencia la conexion del appsettings.json
        public static void ConfigurarConexion(string pConexion)
        {
            Conexion.SetConexion(pConexion);
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

  
    }
}
