using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Microsoft.Data.SqlClient;
using Persistencia.Interfaces;

namespace Persistencia
{
    public class PersistenciaEscribano : IPersistenciaEscribano
    {
        public void Registrar(Escribano pEscribano)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Usuario_RegistrarEscribano", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _nombre = new SqlParameter("@NombreCompleto", pEscribano.NombreCompleto);

            SqlParameter _telefono = new SqlParameter("@Telefono", pEscribano.Telefono);

            SqlParameter _email = new SqlParameter("@Email", pEscribano.Email);

            SqlParameter _pass = new SqlParameter("@Contrasena", pEscribano.Contrasena);

            SqlParameter _numCaja = new SqlParameter("@NumCajaProf", pEscribano.NumeroCaja);

            SqlParameter _direccion = new SqlParameter("@DireccionEstudio", pEscribano.Direccion);

            oComando.Parameters.Add(_nombre);
            oComando.Parameters.Add(_telefono);
            oComando.Parameters.Add(_email);
            oComando.Parameters.Add(_pass);
            oComando.Parameters.Add(_numCaja);
            oComando.Parameters.Add(_direccion);

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

        public Escribano ObtenerPorId(int pIdUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Escribano_ObtenerPorId", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _id = new SqlParameter("@IdUsuario", pIdUsuario);

            oComando.Parameters.Add(_id);

            try
            {
                oConexion.Open();

                SqlDataReader lector =
                    oComando.ExecuteReader();

                if (lector.Read())
                {
                    int id = Convert.ToInt32(lector["IdUsuario"]);

                    string nombre = lector["NombreCompleto"].ToString() ?? string.Empty;

                    string telefono = lector["Telefono"].ToString() ?? string.Empty;

                    string email = lector["Email"].ToString() ?? string.Empty;

                    bool estado = Convert.ToBoolean(lector["Estado"]);

                    string numCaja = lector["NumCajaProf"].ToString() ?? string.Empty;

                    string direccion = lector["DireccionEstudio"].ToString() ?? string.Empty;

                    DateTime? fechaAceptacion = lector["FechaAceptacionTerminos"] == DBNull.Value ? null : Convert.ToDateTime(lector["FechaAceptacionTerminos"]);

                    return new Escribano(id, nombre, telefono, email, "", estado,
                        Rol.Escribano, fechaAceptacion, numCaja, direccion);
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

        public void ActualizarPerfil(Escribano pEscribano)
        {
            // el perfil del escribano tiene campos en dos tablas:
            // primero actualizo los datos de Usuario, despues los datos especificos del Escribano.
            // El NumeroCaja se pasa tal cual viene en pEscribano (el controller lo carga desde la base
            // antes de llamar, asi nunca confiamos en el valor del formulario).
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComandoUsuario = new SqlCommand("sp_Usuario_ActualizarPerfil", oConexion);

            oComandoUsuario.CommandType = CommandType.StoredProcedure;

            SqlParameter _idU = new SqlParameter("@IdUsuario", pEscribano.IdUsuario);

            SqlParameter _nombre = new SqlParameter("@NombreCompleto", pEscribano.NombreCompleto);

            SqlParameter _telefono = new SqlParameter("@Telefono", pEscribano.Telefono);

            SqlParameter _email = new SqlParameter("@Email", pEscribano.Email);

            oComandoUsuario.Parameters.Add(_idU);
            oComandoUsuario.Parameters.Add(_nombre);
            oComandoUsuario.Parameters.Add(_telefono);
            oComandoUsuario.Parameters.Add(_email);

            SqlCommand oComandoEscribano = new SqlCommand("sp_Escribano_ActualizarDatos", oConexion);

            oComandoEscribano.CommandType = CommandType.StoredProcedure;

            SqlParameter _idE = new SqlParameter("@IdUsuario", pEscribano.IdUsuario);

            SqlParameter _numCaja = new SqlParameter("@NumCajaProf", pEscribano.NumeroCaja);

            SqlParameter _direccion = new SqlParameter("@DireccionEstudio", pEscribano.Direccion);

            oComandoEscribano.Parameters.Add(_idE);
            oComandoEscribano.Parameters.Add(_numCaja);
            oComandoEscribano.Parameters.Add(_direccion);

            try
            {
                oConexion.Open();

                oComandoUsuario.ExecuteNonQuery();
                oComandoEscribano.ExecuteNonQuery();
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

        public void Inactivar(int pIdUsuario)
        {
            // reutilizo el SP de admin para setear Estado=0 sobre el Usuario
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Admin_SetEstadoUsuario", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _id = new SqlParameter("@Id", pIdUsuario);

            SqlParameter _estado = new SqlParameter("@Estado", false);

            oComando.Parameters.Add(_id);
            oComando.Parameters.Add(_estado);

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
