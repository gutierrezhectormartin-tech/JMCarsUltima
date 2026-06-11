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

        public LogicaEscribano()
        {
            // Resuelvo la implementacion de la persistencia escribano
            persistenciaEscribano = FabricaPersistencia.GetInstancia().GetPersistenciaEscribano();
        }

        public void Registrar(Escribano pEscribano)
        {
            // ver como encriptar la contraseña

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
            persistenciaEscribano.Inactivar(pIdUsuario);
        }
    }
}
