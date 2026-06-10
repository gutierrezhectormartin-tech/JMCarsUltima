using Logica.Interfaces;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// comentario nomas

namespace Logica
{
    public class FabricaLogica
    {
        private static FabricaLogica _instancia;

        private FabricaLogica() { }

        public static FabricaLogica GetInstancia()
        {
            if (_instancia == null)
                _instancia = new FabricaLogica();

            return _instancia;
        }

        public ILogicaUsuario GetLogicaUsuario()
        {
            return new LogicaUsuario();
        }

        public ILogicaCliente GetLogicaCliente()
        {
            return new LogicaCliente();
        }

        public ILogicaEscribano GetLogicaEscribano()
        {
            return new LogicaEscribano();
        }

        public static ILogicaVehiculo GetLogicaVehiculo()
        {
            return new LogicaVehiculo();
        }
    }
}
