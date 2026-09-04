using Logica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistencia;
using Modelo;
using Persistencia.Interfaces;

namespace Logica
{
    public class LogicaChat : ILogicaChat
    {
        private readonly IPersistenciaChat _persistenciaChat;

        public LogicaChat()
        {
            _persistenciaChat = FabricaPersistencia.GetInstancia().GetPersistenciaChat();
        }

        public List<Chat> ListarChatsPorUsuario(int pIdUsuario)
        {
            return _persistenciaChat.ListarChatsPorUsuario(pIdUsuario);
        }

        public List<Mensaje> ObtenerMensajes(int pIdChat, int pIdUsuario)
        {
            return _persistenciaChat.ObtenerMensajes(pIdChat, pIdUsuario);
        }

        public int ObtenerOCrearChat(int pIdVehiculo, int pIdComprador, int pIdVendedor)
        {
            return _persistenciaChat.ObtenerOCrearChat(pIdVehiculo, pIdComprador, pIdVendedor);
        }

        public void EnviarMensaje(int pIdChat, int pIdEmisor, string pContenido)
        {
            _persistenciaChat.EnviarMensaje(pIdChat, pIdEmisor, pContenido);
        }
    }
}
