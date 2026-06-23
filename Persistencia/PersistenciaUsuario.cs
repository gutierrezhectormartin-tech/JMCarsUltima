using Microsoft.Data.SqlClient;
using Modelo;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
namespace Persistencia
{
    public class PersistenciaUsuario : IPersistenciaUsuario
    {
        public bool ExisteEmail(string pEmail)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Usuario_ExisteEmail", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@Email", pEmail);

            try
            {
                oConexion.Open();

                int cantidad =
                    Convert.ToInt32(
                        oComando.ExecuteScalar());

                return cantidad > 0;
            }
            finally
            {
                oConexion.Close();
            }
        }

        public Usuario Login(string pEmail, string pPass)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Usuario_Login", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _email = new SqlParameter("@Email", pEmail);

            SqlParameter _pass = new SqlParameter("@Contrasena", pPass);

            oComando.Parameters.Add(_email);
            oComando.Parameters.Add(_pass);

            try
            {
                oConexion.Open();

                SqlDataReader oReader =
                    oComando.ExecuteReader();

                if (oReader.Read())
                {
                    int rol =
                        Convert.ToInt32(oReader["IdRol"]);

                    int id =
                        Convert.ToInt32(oReader["IdUsuario"]);

                    string nombre =
                        oReader["NombreCompleto"].ToString();

                    bool estado =
                        Convert.ToBoolean(oReader["Estado"]);

                    if (rol == (int)Rol.Administrador)
                    {
                        return new Administrador(
                            id,
                            nombre ?? string.Empty,
                            "",
                            pEmail,
                            pPass,
                            estado,
                            Rol.Administrador
                        );
                    }
                    else if (rol == (int)Rol.Escribano)
                    {
                        return new Escribano(
                            id,
                            nombre ?? string.Empty,
                            "",
                            pEmail,
                            pPass,
                            estado,
                            Rol.Escribano,
                            "",
                            ""
                        );
                    }
                    else
                    {
                        return new Cliente(
                            id,
                            nombre ?? string.Empty,
                            "",
                            pEmail,
                            pPass,
                            estado,
                            Rol.Cliente,
                            ""
                        );
                    }
                }

                return null!;
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

        public Usuario ObtenerPorEmail (string pEmail)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("ObtenerUsuarioxMail", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@Email", pEmail);

            try
            {
                oConexion.Open();

                SqlDataReader lector = oComando.ExecuteReader();

                if(lector.Read())
                {
                    int id = Convert.ToInt32(lector["IdUsuario"]);
                    string nombre = lector["NombreCompleto"].ToString() ?? string.Empty;
                    string telefono = lector["Telefono"].ToString() ?? string.Empty;
                    string email = lector["Email"].ToString() ?? string.Empty;
                    bool estado = Convert.ToBoolean(lector["Estado"]);
                    int rol = Convert.ToInt32(lector["IdRol"]);

                    Usuario usuario;

                    if(rol == (int)Rol.Administrador)
                    {
                        usuario = new Administrador(id, nombre, telefono, email, "", estado, Rol.Administrador);
                    }
                    else if(rol == (int)Rol.Escribano)
                    {
                        usuario = new Escribano(id, nombre, telefono, email, "", estado, Rol.Escribano, "", "");
                    }
                    else
                    {
                        usuario = new Cliente(id, nombre, telefono, email, "", estado, Rol.Cliente, "");
                    }
                    return usuario;
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

        public void ActualizarContrasena(int pIdUsuario, string pNuevaContrasena)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("ActualizarContrasenaUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdUsuario", pIdUsuario);
            oComando.Parameters.AddWithValue("@NuevaContrasena", pNuevaContrasena);

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