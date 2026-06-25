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
        Usuario Login(string pEmail);

        Usuario ObtenerPorEmail(string pEmail);

        void ActualizarContrasena(int pIdUsuario, string pNuevaContrasena);
        bool ExisteEmail(string pEmail);
    }
}
