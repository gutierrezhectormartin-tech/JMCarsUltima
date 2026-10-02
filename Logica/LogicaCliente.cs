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
        private IPersistenciaUsuario persistenciaUsuario;

        public LogicaCliente()
        {
            persistenciaCliente = FabricaPersistencia.GetInstancia().GetPersistenciaCliente();
            persistenciaUsuario = FabricaPersistencia.GetInstancia().GetPersistenciaUsuario();
        }

        public void Registrar(Cliente pCliente)
        {
            // ver como encriptar la contraseña
            pCliente.Contrasena = Encriptacion.Hashear(pCliente.Contrasena!);
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
            Cliente unCliente = persistenciaCliente.ObtenerPorId(pIdUsuario);

            if (unCliente == null)
            {
                throw new Exception("El cliente no existe.");
            }

            if (!unCliente.EstadoUsu)
            {
                throw new Exception("La cuenta del cliente ya se encuentra inactiva.");
            }

            if (persistenciaCliente.TieneOperacionesEnCurso(pIdUsuario))
            {
                throw new Exception("No se puede inactivar la cuenta porque tiene solicitudes notariales o compraventas en curso.");
            }

            if (persistenciaCliente.TieneVehiculosActivos(pIdUsuario))
            {
                throw new Exception("No se puede inactivar la cuenta porque tiene vehículos publicados, pendientes de aprobación o en trámite. Primero hay que inactivarlos.");
            }

            persistenciaCliente.Inactivar(pIdUsuario);
        }

        public void Activar(int pIdUsuario)
        {
            Cliente unCliente = persistenciaCliente.ObtenerPorId(pIdUsuario);

            if (unCliente == null)
            {
                throw new Exception("El cliente no existe.");
            }

            if (unCliente.EstadoUsu)
            {
                throw new Exception("La cuenta del cliente ya se encuentra activa.");
            }

            if (persistenciaUsuario.ExisteEmail(unCliente.Email))
            {
                throw new Exception("No se puede activar la cuenta porque ya existe otra cuenta activa con el email " + unCliente.Email + ".");
            }

            if (persistenciaCliente.ExisteCedulaActiva(unCliente.Cedula))
            {
                throw new Exception("No se puede activar la cuenta porque ya existe otro cliente activo con la cédula " + unCliente.Cedula + ".");
            }

            persistenciaCliente.Activar(pIdUsuario);
        }

        public List<Cliente> ListarTodos()
        {
            return persistenciaCliente.ListarTodos();
        }
    }
}
