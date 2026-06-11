using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Interfaces
{
    public interface ILogicaCliente
    {
        void Registrar(Cliente pCliente);

        Cliente ObtenerPorId(int pIdUsuario);

        void ActualizarPerfil(Cliente pCliente);

        void Inactivar(int pIdUsuario);
    }
}
