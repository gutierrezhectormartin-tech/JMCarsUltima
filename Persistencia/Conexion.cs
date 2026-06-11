using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Persistencia
{
    internal class Conexion
    {
        // la conexion se setea una unica vez al inicio de la aplicacion
        // desde FabricaPersistencia.ConfigurarConexion (llamado por WebApi/Program.cs)
        private static string _conexionBd;

        internal static void SetConexion(string pConexion)
        {
            _conexionBd = pConexion;
        }

        public static string GetConexion()
        {
            return _conexionBd;
        }
    }
}
