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
    public class LogicaUsuario : ILogicaUsuario
    {
        private IPersistenciaUsuario persistenciaUsuario;

        public LogicaUsuario()
        {
            persistenciaUsuario = FabricaPersistencia.GetInstancia().GetPersistenciaUsuario();
        }

        public Usuario Login(string pEmail, string pPass)
        {
            return persistenciaUsuario.Login(pEmail, pPass);
        }

        public bool ExisteEmail(string pEmail)
        {
            return persistenciaUsuario.ExisteEmail(pEmail);
        }
    }
}
