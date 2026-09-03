using Modelo;
using System.Collections.Generic;

namespace Logica.Interfaces
{
    public interface ILogicaSolicitudNotarial
    {
        void Crear(int pIdCliente, int pIdVehiculo, int pIdEscribano);

        void Aceptar(int pIdSolicitud, int pIdEscribano);

        void Rechazar(int pIdSolicitud, int pIdEscribano);

        void Finalizar(int pIdSolicitud, int pIdEscribano);

        SolicitudEscribano ObtenerPorId(int pIdSolicitud);

        List<SolicitudEscribano> ListarPorCliente(int pIdCliente);

        List<SolicitudEscribano> ListarPorEscribano(int pIdEscribano);
    }
}
