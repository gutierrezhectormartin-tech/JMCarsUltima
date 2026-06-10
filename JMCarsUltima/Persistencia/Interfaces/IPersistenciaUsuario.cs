using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces
{
    public interface IPersistenciaUsuario
    {
        Usuario Login(string pEmail, string pPass);
        bool ExisteEmail(string pEmail);
    }
}
