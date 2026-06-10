using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class CompraVenta
    {
        private int idCompraVenta;
        private DateTime fechaInicio;
        private int estadoCompraVenta;
        private SolicituNotarial solicitud;

        public int IdCompraVenta
        {
            get { return idCompraVenta; }
            set
            {
                idCompraVenta = value;
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

        public int EstadoCompraVenta
        {
            get { return estadoCompraVenta; }
            set
            {
                estadoCompraVenta = value;
            }
        }

        public SolicituNotarial Solicitud
        {
            get { return solicitud; }
            set
            {
                solicitud = value;
            }
        }

        public CompraVenta(int pIdCompraVenta, DateTime pFechaInicio, int pEstadoCompraVenta, SolicituNotarial pSolicitud)
        {
            IdCompraVenta = pIdCompraVenta;
            FechaInicio = pFechaInicio;
            EstadoCompraVenta = pEstadoCompraVenta;
            Solicitud = pSolicitud;
        }
    }
}
