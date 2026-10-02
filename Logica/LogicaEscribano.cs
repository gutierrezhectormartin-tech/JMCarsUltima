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
    public class LogicaEscribano : ILogicaEscribano
    {
        // Referencia a la persistencia escribano
        private IPersistenciaEscribano persistenciaEscribano;
        private IPersistenciaUsuario persistenciaUsuario;

        public LogicaEscribano()
        {
            // Resuelvo la implementacion de la persistencia escribano
            persistenciaEscribano = FabricaPersistencia.GetInstancia().GetPersistenciaEscribano();
            persistenciaUsuario = FabricaPersistencia.GetInstancia().GetPersistenciaUsuario();
        }

        public void Registrar(Escribano pEscribano)
        {
            // ver como encriptar la contraseña
            pEscribano.Contrasena = Encriptacion.Hashear(pEscribano.Contrasena!);
            persistenciaEscribano.Registrar(pEscribano);
        }

        public Escribano ObtenerPorId(int pIdUsuario)
        {
            return persistenciaEscribano.ObtenerPorId(pIdUsuario);
        }

        public void ActualizarPerfil(Escribano pEscribano)
        {
            persistenciaEscribano.ActualizarPerfil(pEscribano);
        }

        public void Inactivar(int pIdUsuario)
        {
            Escribano unEscribano = persistenciaEscribano.ObtenerPorId(pIdUsuario);

            if (unEscribano == null)
            {
                throw new Exception("El escribano no existe.");
            }

            if (!unEscribano.EstadoUsu)
            {
                throw new Exception("La cuenta del escribano ya se encuentra inactiva.");
            }

            // no se puede inactivar si tiene solicitudes o compraventas asignadas sin terminar
            if (persistenciaEscribano.TieneOperacionesEnCurso(pIdUsuario))
            {
                throw new Exception("No se puede inactivar la cuenta porque tiene solicitudes notariales o compraventas en curso.");
            }

            persistenciaEscribano.Inactivar(pIdUsuario);
        }

        public List<Escribano> ListarActivos()
        {
            return persistenciaEscribano.ListarActivos();
        }

        public void Activar(int pIdUsuario)
        {
            Escribano unEscribano = persistenciaEscribano.ObtenerPorId(pIdUsuario);

            if (unEscribano == null)
            {
                throw new Exception("El escribano no existe.");
            }

            if (unEscribano.EstadoUsu)
            {
                throw new Exception("El escribano ya se encuentra activo.");
            }

            // el email pudo haber quedado en uso por otra cuenta activa
            if (persistenciaUsuario.ExisteEmail(unEscribano.Email))
            {
                throw new Exception("No se puede activar la cuenta porque ya existe otra cuenta activa con el email " + unEscribano.Email + ".");
            }

            persistenciaEscribano.Activar(pIdUsuario);
        }

        public List<Escribano> ListarTodos()
        {
            return persistenciaEscribano.ListarTodos();
        }
    }
}
