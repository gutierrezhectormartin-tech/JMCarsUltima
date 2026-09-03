using Logica.Interfaces;
using Modelo;
using Persistencia;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logica
{
    public class LogicaSolicitudNotarial : ILogicaSolicitudNotarial
    {
        private IPersistenciaSolicitudNotarial _persistenciaSolicitud;
        private IPersistenciaVehiculo _persistenciaVehiculo;

        public LogicaSolicitudNotarial()
        {
            _persistenciaSolicitud = FabricaPersistencia.GetInstancia().GetPersistenciaSolicitudNotarial();
            _persistenciaVehiculo = FabricaPersistencia.GetInstancia().GetPersistenciaVehiculo();
        }

        public void Crear(int pIdCliente, int pIdVehiculo, int pIdEscribano)
        {
            Vehiculo unVehiculo = _persistenciaVehiculo.DetalleVehiculo(pIdVehiculo);

            if (unVehiculo == null)
            {
                throw new Exception("El vehículo solicitado no existe.");
            }

            if (!unVehiculo.Publicado)
            {
                throw new Exception("El vehículo no está publicado.");
            }

            if (unVehiculo.Vendedor.IdUsuario == pIdCliente)
            {
                throw new Exception("No puedes solicitar un escribano para tu propio vehículo.");
            }

            List<SolicitudEscribano> misSolicitudes = _persistenciaSolicitud.ListarPorCliente(pIdCliente);

            bool tieneSolicitudActiva = misSolicitudes.Any(s =>
                s.Solicitud.Vehiculo.IdVehiculo == pIdVehiculo &&
                (s.Solicitud.EstadoSolicitud == 1 || s.Solicitud.EstadoSolicitud == 2));

            if (tieneSolicitudActiva)
            {
                throw new Exception("Ya tienes una solicitud activa sobre este vehículo.");
            }

            try
            {
                _persistenciaSolicitud.Crear(pIdCliente, pIdVehiculo, pIdEscribano);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica al crear la solicitud: " + ex.Message);
            }
        }

        public void Aceptar(int pIdSolicitud, int pIdEscribano)
        {
            try
            {
                _persistenciaSolicitud.Aceptar(pIdSolicitud, pIdEscribano);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica al aceptar la solicitud: " + ex.Message);
            }
        }

        public void Rechazar(int pIdSolicitud, int pIdEscribano)
        {
            try
            {
                _persistenciaSolicitud.Rechazar(pIdSolicitud, pIdEscribano);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica al rechazar la solicitud: " + ex.Message);
            }
        }

        public void Finalizar(int pIdSolicitud, int pIdEscribano)
        {
            // sp_Notarial_FinalizarVenta no recibe el escribano, así que la pertenencia se valida acá.
            SolicitudEscribano solicitud = _persistenciaSolicitud.ObtenerPorId(pIdSolicitud);

            if (solicitud == null)
            {
                throw new Exception("La solicitud no existe.");
            }

            if (solicitud.Escribano.IdUsuario != pIdEscribano)
            {
                throw new Exception("No tienes permiso para finalizar esta solicitud.");
            }

            try
            {
                _persistenciaSolicitud.Finalizar(pIdSolicitud);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica al finalizar la solicitud: " + ex.Message);
            }
        }

        public SolicitudEscribano ObtenerPorId(int pIdSolicitud)
        {
            return _persistenciaSolicitud.ObtenerPorId(pIdSolicitud);
        }

        public List<SolicitudEscribano> ListarPorCliente(int pIdCliente)
        {
            return _persistenciaSolicitud.ListarPorCliente(pIdCliente);
        }

        public List<SolicitudEscribano> ListarPorEscribano(int pIdEscribano)
        {
            return _persistenciaSolicitud.ListarPorEscribano(pIdEscribano);
        }
    }
}
