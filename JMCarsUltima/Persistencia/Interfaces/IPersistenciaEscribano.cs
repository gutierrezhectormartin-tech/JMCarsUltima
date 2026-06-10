using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces
{
    public interface IPersistenciaEscribano
    {
        void Registrar(Escribano pEscribano);

        Escribano ObtenerPorId(int pIdUsuario);

        void ActualizarPerfil(Escribano pEscribano);

        void Inactivar(int pIdUsuario);
    }
}
