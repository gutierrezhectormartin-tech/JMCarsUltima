using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class SolicituNotarial
    {
        private int idSolicitud;
        private DateTime fechaSolicitud;
        private int estadoSolicitud;
        private Cliente cliente;
        private Vehiculo vehiculo;

        public int IdSolicitud
        {
            get { return idSolicitud; }
            set
            {
                idSolicitud = value;
            }
        }

        public DateTime FechaSolicitud
        {
            get { return fechaSolicitud; }
            set
            {
                fechaSolicitud = value;
            }
        }

        public int EstadoSolicitud
        {
            get { return estadoSolicitud; }
            set
            {
                estadoSolicitud = value;
            }
        }

        public Cliente Cliente
        {
            get { return cliente; }
            set
            {
                cliente = value;
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

        public SolicituNotarial(int pIdSolicitud, DateTime pFechaSolicitud, int pEstadoSolicitud, Cliente pCliente, Vehiculo pVehiculo)
        {
            IdSolicitud = pIdSolicitud;
            FechaSolicitud = pFechaSolicitud;
            EstadoSolicitud = pEstadoSolicitud;
            Cliente = pCliente;
            Vehiculo = pVehiculo;
        }
    }
}
