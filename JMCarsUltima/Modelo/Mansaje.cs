using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Mansaje
    {
        private int idMensaje;
        private string contenido;
        private DateTime fechaHora;
        private Usuario usuarioEmisor;
        private Chat chat;

        public int IdMensaje
        {
            get { return idMensaje; }
            set
            {
                idMensaje = value;
            }
        }

        public Chat Chat
        {
            get { return chat; }
            set
            {
                chat = value;
            }
        }

        public Usuario UsuarioEmisor
        {
            get { return usuarioEmisor; }
            set
            {
                usuarioEmisor = value;
            }
        }

        public string Contenido
        {
            get { return contenido; }
            set
            {
                contenido = value;
            }
        }

        public DateTime FechaHora
        {
            get { return fechaHora; }
            set
            {
                fechaHora = value;
            }
        }

        public Mansaje(int pIdMensaje, Chat pChat, Usuario pUsuarioEmisor, string pContenido, DateTime pFechaHora)
        {
            IdMensaje = pIdMensaje;
            Chat = pChat;
            UsuarioEmisor = pUsuarioEmisor;
            Contenido = pContenido;
            FechaHora = pFechaHora;
        }
    }
}
