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

        public Chat(int pIdChat, DateTime pFechaInicio, Vehiculo pVehiculo)
        {
            IdChat = pIdChat;
            FechaInicio = pFechaInicio;
            Vehiculo = pVehiculo;
        }
    }
}
