using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Chat
    {
        private int idChat;
        private DateTime fechaInicio;
        private Vehiculo vehiculo;
        private string nombreOtroUsuario;
        private string ultimoMensaje;
        private DateTime? fechaUltimoMensaje;


        public int IdChat
        {
            get { return idChat; }
            set
            {
                idChat = value;
            }
        }

        public DateTime FechaInicio
        {
            get { return fechaInicio; }
            set
            {
                fechaInicio = value;
            }
        }

        public Vehiculo Vehiculo
        {
            get { return vehiculo; }
            set
            {
                vehiculo = value;
            }
        }

        public string NombreOtroUsuario
        {
            get { return nombreOtroUsuario; }
            set
            {
                NombreOtroUsuario = value;
            }
        }

        public string UltimoMensaje
        {
            get { return ultimoMensaje; }
            set
            {
                UltimoMensaje = value;
            }
        }

        public DateTime? FechaUltimoMensaje
        {
            get { return fechaUltimoMensaje; }
            set
            {
                fechaUltimoMensaje = value;
            }
        }
        public Chat(int pIdChat, DateTime pFechaInicio, Vehiculo pVehiculo, string pOtroUsuario, string pUltimoMensaje, DateTime? pFechaUltimoMensaje)
        {
            IdChat = pIdChat;
            FechaInicio = pFechaInicio;
            Vehiculo = pVehiculo;
            NombreOtroUsuario = pOtroUsuario;
            UltimoMensaje = pUltimoMensaje;
            FechaUltimoMensaje = pFechaUltimoMensaje;
        }

        public Chat() { }
    }
}
