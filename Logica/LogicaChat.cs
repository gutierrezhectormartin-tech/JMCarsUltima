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
        private readonly IPersistenciaRegistroActividad _persistenciaRegistroActividad;
        public LogicaChat()
        {
            _persistenciaChat = FabricaPersistencia.GetInstancia().GetPersistenciaChat();
            _persistenciaRegistroActividad = FabricaPersistencia.GetInstancia().GetPersistenciaRegistroActividad();
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
            if(pIdComprador == pIdVendedor)
            {
                throw new Exception("No se puede crear una conversacion contigo mismo");
            }
            return _persistenciaChat.ObtenerOCrearChat(pIdVehiculo, pIdComprador, pIdVendedor);
        }

        public void EnviarMensaje(int pIdChat, int pIdEmisor, string pContenido)
        {
            _persistenciaChat.EnviarMensaje(pIdChat, pIdEmisor, pContenido);
            try
            {
                _persistenciaRegistroActividad.RegistrarActividad(pIdEmisor, "Envia Mensaje", $"Mensaje enviado al chat id {pIdChat}");
            }
            catch (Exception)
            {
            }
        }
        public List<Chat> ListarChatsPorVehiculo(int pIdVehiculo)
        {
            return _persistenciaChat.ListarChatsPorVehiculo(pIdVehiculo);
        }
    }
}
