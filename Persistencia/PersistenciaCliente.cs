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

            SqlParameter _fecha = new SqlParameter("@FechaAceptacionTerminos", pCliente.FechaAceptacionTerminos ?? (object)DateTime.Now);

            oComando.Parameters.Add(_nombre);
            oComando.Parameters.Add(_telefono);
            oComando.Parameters.Add(_email);
            oComando.Parameters.Add(_pass);
            oComando.Parameters.Add(_cedula);
            oComando.Parameters.Add(_fecha);

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

                SqlDataReader lector = oComando.ExecuteReader();

                if (lector.Read())
                {
                    int id = Convert.ToInt32(lector["IdUsuario"]);

                    string nombre = lector["NombreCompleto"].ToString() ?? string.Empty;

                    string telefono = lector["Telefono"].ToString() ?? string.Empty;

                    string email = lector["Email"].ToString() ?? string.Empty;

                    bool estado = Convert.ToBoolean(lector["Estado"]);

                    string cedula = lector["Cedula"].ToString() ?? string.Empty;

                    DateTime? fechaAceptacion = lector["FechaAceptacionTerminos"] == DBNull.Value ? null : Convert.ToDateTime(lector["FechaAceptacionTerminos"]);

                    return new Cliente(
                        id,
                        nombre,
                        telefono,
                        email,
                        "",
                        estado,
                        Rol.Cliente,
                        fechaAceptacion,
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

        public void Activar(int pIdUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Admin_SetEstadoUsuario", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _id = new SqlParameter("@Id", pIdUsuario);

            SqlParameter _estado = new SqlParameter("@Estado", true);

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

        public List<Cliente> ListarTodos()
        {
            List<Cliente> lista = new List<Cliente>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Cliente_ListarTodos", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();

                SqlDataReader lector = oComando.ExecuteReader();

                while (lector.Read())
                {
                    DateTime? fechaAceptacion = lector["FechaAceptacionTerminos"] == DBNull.Value ? null : Convert.ToDateTime(lector["FechaAceptacionTerminos"]);

                    Cliente unCliente = new Cliente(
                        Convert.ToInt32(lector["IdUsuario"]),
                        lector["NombreCompleto"].ToString() ?? string.Empty,
                        lector["Telefono"].ToString() ?? string.Empty,
                        lector["Email"].ToString() ?? string.Empty,
                        "", Convert.ToBoolean(lector["Estado"]), Rol.Cliente, fechaAceptacion,
                        lector["Cedula"].ToString() ?? string.Empty);

                    lista.Add(unCliente);
                }

                lector.Close();

                return lista;
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

        public bool TieneVehiculosActivos(int pIdUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Cliente_TieneVehiculosActivos", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _id = new SqlParameter("@IdUsuario", pIdUsuario);

            oComando.Parameters.Add(_id);

            try
            {
                oConexion.Open();

                int cantidad = Convert.ToInt32(oComando.ExecuteScalar());

                return cantidad > 0;
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

        public bool TieneOperacionesEnCurso(int pIdUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Cliente_TieneOperacionesEnCurso", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _id = new SqlParameter("@IdUsuario", pIdUsuario);

            oComando.Parameters.Add(_id);

            try
            {
                oConexion.Open();

                int cantidad = Convert.ToInt32(oComando.ExecuteScalar());

                return cantidad > 0;
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

        public bool ExisteCedulaActiva(string pCedula)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Cliente_ExisteCedulaActiva", oConexion);

            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _cedula = new SqlParameter("@Cedula", pCedula);

            oComando.Parameters.Add(_cedula);

            try
            {
                oConexion.Open();

                int cantidad = Convert.ToInt32(oComando.ExecuteScalar());

                return cantidad > 0;
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
