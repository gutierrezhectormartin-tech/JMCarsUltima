using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Interfaces
{
    public interface ILogicaEscribano
    {
        void Registrar(Escribano pEscribano);

        Escribano ObtenerPorId(int pIdUsuario);

        void ActualizarPerfil(Escribano pEscribano);

        void Inactivar(int pIdUsuario);
    }
}
