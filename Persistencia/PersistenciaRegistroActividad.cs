using Microsoft.Data.SqlClient;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public  class PersistenciaRegistroActividad : IPersistenciaRegistroActividad
    {
        public void RegistrarActividad(int pIdUsuario, string pTipoAccion, string? pDetalle)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_RegistroActividad_crear", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@IdUsuario", pIdUsuario);
            oComando.Parameters.AddWithValue("@TipoAccion", pTipoAccion);
            oComando.Parameters.AddWithValue("@Detalle", pDetalle ?? (object)DBNull.Value);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oConexion.Close();
            }


        }
    }
}
