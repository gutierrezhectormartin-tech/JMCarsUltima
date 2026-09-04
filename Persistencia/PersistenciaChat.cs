using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Modelo;
using System.Data;

namespace Persistencia
{
    public class PersistenciaChat : IPersistenciaChat
    {
        public List<Chat> ListarChatsPorUsuario(int pIdUsuario)
        {
            List<Chat> lista = new List<Chat>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_ListarChatPorUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@IdUsuario", pIdUsuario);

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while(lector.Read())
                {
                    Marcas marca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());
                    Modelos modelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(), marca);

                    Vehiculo vehiculo = new Vehiculo();

                    vehiculo.IdVehiculo = Convert.ToInt32(lector["IdVehiculo"]);
                    vehiculo.Modelo = modelo;
                    DateTime ? fechaUltimo = lector["FechaUltimoMensaje"] == DBNull.Value ? null : Convert.ToDateTime(lector["FechaUltimoMensaje"]);

                    Chat chat = new Chat(Convert.ToInt32(lector["IdChat"]), Convert.ToDateTime(lector["FechaInicio"]),
                                          vehiculo, lector["NombreOtroUsuario"].ToString(), lector["UltimoMensaje"] == DBNull.Value ? "" : lector["UltimoMensaje"].ToString(),
                                          fechaUltimo);
                    lista.Add(chat);
                }
                return lista;
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

        public List<Mensaje> ObtenerMensajes(int pIdChat, int pIdUsuario)
        {
            List<Mensaje> lista = new List<Mensaje>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_Obtener_Mensajes_Chat", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@IdChat", pIdChat);
            oComando.Parameters.AddWithValue("@IdUsuario", pIdUsuario);

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while(lector.Read())
                {
                    Usuario emisor = new Usuario();
                    emisor.IdUsuario = Convert.ToInt32(lector["IdUsuarioEmisor"]);
                    emisor.NombreCompleto = lector["NombreEmisor"].ToString();

                    Chat chat = new Chat();
                    chat.IdChat = Convert.ToInt32(lector["IdChat"]);

                    Mensaje mensaje = new Mensaje(Convert.ToInt32(lector["IdMensaje"]), chat, emisor, lector["Contenido"].ToString(), Convert.ToDateTime(lector["FechaHora"]));

                    lista.Add(mensaje);
                }
                return lista;
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

        public int ObtenerOCrearChat(int pIdVehiculo, int pIdComprador, int pIdVendedor)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_Chat_Obtener", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@IdVehiculo", pIdVehiculo);
            oComando.Parameters.AddWithValue("@IdComprador", pIdComprador);
            oComando.Parameters.AddWithValue("@IdVendedor", pIdVendedor);

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                if(lector.Read())
                {
                    return Convert.ToInt32(lector["IdChat"]);
                }
                return 0;
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

        public void EnviarMensaje(int pIdChat, int pIdEmisor, string pContenido)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_Mensaje_Enviar", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@IdChat", pIdChat);
            oComando.Parameters.AddWithValue("@IdEmisor", pIdEmisor);
            oComando.Parameters.AddWithValue("@Texto", pContenido);

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
