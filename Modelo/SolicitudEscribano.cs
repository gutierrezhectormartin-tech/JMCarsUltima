using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class SolicitudEscribano
    {
        private SolicituNotarial solicitud;
        private Escribano escribano;

        public SolicituNotarial Solicitud
        {
            get { return solicitud; }
            set
            {
                solicitud = value;
            }
        }

        public Escribano Escribano
        {
            get { return escribano; }
            set
            {
                escribano = value;
            }
        }

        public SolicitudEscribano(SolicituNotarial pSolicitud, Escribano pEscribano)
        {
            Solicitud = pSolicitud;
            Escribano = pEscribano;
        }
    }
}
