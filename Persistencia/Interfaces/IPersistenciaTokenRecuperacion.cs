using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces
{
    public interface IPersistenciaTokenRecuperacion
    {
        void Crear(TokenRecuperacion pToken);
        TokenRecuperacion ObtenerValido(string pToken);
        void MarcarUsado(int pIdToken);
    }
}
