using Modelo;
using System.Collections.Generic;

namespace Persistencia.Interfaces
{
    public interface IPersistenciaSolicitudNotarial
    {
        void Crear(int pIdCliente, int pIdVehiculo, int pIdEscribano);

        void Aceptar(int pIdSolicitud, int pIdEscribano);

        void Rechazar(int pIdSolicitud, int pIdEscribano);

        void Finalizar(int pIdSolicitud);

        SolicitudEscribano ObtenerPorId(int pIdSolicitud);

        List<SolicitudEscribano> ListarPorCliente(int pIdCliente);

        List<SolicitudEscribano> ListarPorEscribano(int pIdEscribano);
    }
}
