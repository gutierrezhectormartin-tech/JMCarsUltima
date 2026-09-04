using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;

namespace Persistencia.Interfaces
{
    public interface IPersistenciaChat
    {
        List<Chat> ListarChatsPorUsuario(int pIdUsuario);
        List<Mensaje> ObtenerMensajes(int pIdChat, int pIdUsuario);
        int ObtenerOCrearChat(int pIdVehiculo, int pIdComprador, int pIdVendedor);
        void EnviarMensaje(int pIdChat, int pIdEmisor, string pContenido);
    }
}
