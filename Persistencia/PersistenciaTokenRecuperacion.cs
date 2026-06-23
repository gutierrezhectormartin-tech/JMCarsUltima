using Microsoft.Data.SqlClient;
using Modelo;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    internal class PersistenciaTokenRecuperacion : IPersistenciaTokenRecuperacion
    {
        public void Crear(TokenRecuperacion pToken)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("TokenRecuperacion", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdUsuario", pToken.IdUsuario);
            oComando.Parameters.AddWithValue("@Token", pToken.Token);
            oComando.Parameters.AddWithValue("@FechaExpiracion", pToken.FechaExpiracion);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }

            finally
            {
                oConexion.Close();
            }
        }

        public TokenRecuperacion ObtenerValido(string pToken)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("TokenRecuperacionObtener", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@Token", pToken);

            try
            {
                oConexion.Open();

                SqlDataReader lector = oComando.ExecuteReader();

                if(lector.Read())
                {
                    return new TokenRecuperacion(Convert.ToInt32(lector["IdToken"]),
                                                 Convert.ToInt32(lector["IdUsuario"]),
                                                 lector["Token"].ToString(),
                                                 Convert.ToDateTime(lector["FechaCreacion"]),
                                                 Convert.ToDateTime(lector["FechaExpiracion"]),
                                                 Convert.ToBoolean(lector["Usado"])
                                                );
                }

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                oConexion.Close();
            }
        }
        public void MarcarUsado(int pIdToken)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("TokenRecuperacionUsado", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdToken", pIdToken);

            try
            {
                oConexion.Open();

                oComando.ExecuteNonQuery();
            }
            catch (Exception ex)
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
