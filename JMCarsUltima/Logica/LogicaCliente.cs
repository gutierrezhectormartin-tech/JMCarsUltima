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
    public class LogicaCliente : ILogicaCliente
    {
        private IPersistenciaCliente persistenciaCliente;

        public LogicaCliente()
        {
            persistenciaCliente = FabricaPersistencia.GetInstancia().GetPersistenciaCliente();
        }

        public void Registrar(Cliente pCliente)
        {
            // ver como encriptar la contraseña

            persistenciaCliente.Registrar(pCliente);
        }

        public Cliente ObtenerPorId(int pIdUsuario)
        {
            return persistenciaCliente.ObtenerPorId(pIdUsuario);
        }

        public void ActualizarPerfil(Cliente pCliente)
        {
            persistenciaCliente.ActualizarPerfil(pCliente);
        }

        public void Inactivar(int pIdUsuario)
        {
            persistenciaCliente.Inactivar(pIdUsuario);
        }
    }
}
