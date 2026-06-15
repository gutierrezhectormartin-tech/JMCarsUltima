using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Persistencia
{
    internal class Conexion
    {
        private static string _conexionBd = "Data Source=.;Initial Catalog=JMCars;Integrated Security=True;TrustServerCertificate=True;";

        public static string GetConexion()
        {
            return _conexionBd;
        }
    }
}
