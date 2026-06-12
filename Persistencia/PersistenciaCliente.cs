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
    public class PersistenciaCliente : IPersistenciaCliente
    {
        public void Registrar(Cliente pCliente)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Usuario_RegistrarCliente", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _nombre = new SqlParameter("@NombreCompleto", pCliente.NombreCompleto);

            SqlParameter _telefono = new SqlParameter("@Telefono", pCliente.Telefono);

            SqlParameter _email = new SqlParameter("@Email", pCliente.Email);

            SqlParameter _pass = new SqlParameter("@Contrasena", pCliente.Contrasena);

            SqlParameter _cedula = new SqlParameter("@Cedula", pCliente.Cedula);

            oComando.Parameters.Add(_nombre);
            oComando.Parameters.Add(_telefono);
            oComando.Parameters.Add(_email);
            oComando.Parameters.Add(_pass);
            oComando.Parameters.Add(_cedula);

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

        public Cliente ObtenerPorId(int pIdUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Cliente_ObtenerPorId", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _id = new SqlParameter("@IdUsuario", pIdUsuario);

            oComando.Parameters.Add(_id);

            try
            {
                oConexion.Open();

                SqlDataReader oReader =
                    oComando.ExecuteReader();

                if (oReader.Read())
                {
                    int id =
                        Convert.ToInt32(oReader["IdUsuario"]);

                    string nombre =
                        oReader["NombreCompleto"].ToString();

                    string telefono =
                        oReader["Telefono"].ToString();

                    string email =
                        oReader["Email"].ToString();

                    bool estado =
                        Convert.ToBoolean(oReader["Estado"]);

                    string cedula =
                        oReader["Cedula"].ToString();

                    return new Cliente(
                        id,
                        nombre,
                        telefono,
                        email,
                        "",
                        estado,
                        Rol.Cliente,
                        cedula
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

        public void ActualizarPerfil(Cliente pCliente)
        {
            // los campos editables del cliente viven todos en la tabla Usuario,
            // por eso alcanza con un unico SP. La Cedula no es editable.
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Usuario_ActualizarPerfil", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _id = new SqlParameter("@IdUsuario", pCliente.IdUsuario);

            SqlParameter _nombre = new SqlParameter("@NombreCompleto", pCliente.NombreCompleto);

            SqlParameter _telefono = new SqlParameter("@Telefono", pCliente.Telefono);

            SqlParameter _email = new SqlParameter("@Email", pCliente.Email);

            oComando.Parameters.Add(_id);
            oComando.Parameters.Add(_nombre);
            oComando.Parameters.Add(_telefono);
            oComando.Parameters.Add(_email);

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
