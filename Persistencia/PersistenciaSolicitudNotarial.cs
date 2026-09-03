using Microsoft.Data.SqlClient;
using Modelo;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Persistencia
{
    public class PersistenciaSolicitudNotarial : IPersistenciaSolicitudNotarial
    {
        public void Crear(int pIdCliente, int pIdVehiculo, int pIdEscribano)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Notarial_CrearSolicitud", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdCliente", pIdCliente);
            oComando.Parameters.AddWithValue("@IdVehiculo", pIdVehiculo);
            oComando.Parameters.AddWithValue("@IdEscribano", pIdEscribano);

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

        public void Aceptar(int pIdSolicitud, int pIdEscribano)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Notarial_Aceptar", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdSolicitud", pIdSolicitud);
            oComando.Parameters.AddWithValue("@IdEscribano", pIdEscribano);

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

        public void Rechazar(int pIdSolicitud, int pIdEscribano)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Notarial_Rechazar", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdSolicitud", pIdSolicitud);
            oComando.Parameters.AddWithValue("@IdEscribano", pIdEscribano);

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

        public void Finalizar(int pIdSolicitud)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Notarial_FinalizarVenta", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdSolicitud", pIdSolicitud);

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

        public SolicitudEscribano ObtenerPorId(int pIdSolicitud)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Notarial_ObtenerPorId", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdSolicitud", pIdSolicitud);

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                SolicitudEscribano resultado = null;

                if (lector.Read())
                {
                    Marcas unaMarca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(), unaMarca);

                    Cliente unVendedor = new Cliente(Convert.ToInt32(lector["IdVendedor"]),
                        lector["NombreVendedor"].ToString() ?? string.Empty,
                        lector["TelefonoVendedor"].ToString() ?? string.Empty,
                        lector["EmailVendedor"].ToString() ?? string.Empty,
                        "", true, Rol.Cliente, null, lector["CedulaVendedor"].ToString() ?? string.Empty);

                    decimal? latitud = lector["Latitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Latitud"]);
                    decimal? longitud = lector["Longitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Longitud"]);

                    Vehiculo unVehiculo = new Vehiculo(Convert.ToInt32(lector["IdVehiculo"]),
                        Convert.ToDecimal(lector["Precio"]),
                        Convert.ToInt32(lector["Kilometraje"]),
                        Convert.ToInt32(lector["Ano"]),
                        lector["CajaDeCambios"].ToString() ?? string.Empty,
                        lector["Motorizacion"].ToString() ?? string.Empty,
                        lector["Descripcion"].ToString() ?? string.Empty,
                        Convert.ToBoolean(lector["Publicado"]),
                        latitud,
                        longitud,
                        unModelo,
                        unVendedor,
                        new List<string>());

                    Cliente unCliente = new Cliente(Convert.ToInt32(lector["IdCliente"]),
                        lector["NombreCliente"].ToString() ?? string.Empty,
                        lector["TelefonoCliente"].ToString() ?? string.Empty,
                        lector["EmailCliente"].ToString() ?? string.Empty,
                        "", true, Rol.Cliente, null, lector["Cedula"].ToString() ?? string.Empty);

                    SolicituNotarial unaSolicitud = new SolicituNotarial(
                        Convert.ToInt32(lector["IdSolicitud"]),
                        Convert.ToDateTime(lector["FechaSolicitud"]),
                        Convert.ToInt32(lector["EstadoSolicitud"]),
                        unCliente,
                        unVehiculo);

                    Escribano unEscribano = new Escribano(Convert.ToInt32(lector["IdEscribano"]),
                        lector["NombreEscribano"].ToString() ?? string.Empty,
                        lector["TelefonoEscribano"].ToString() ?? string.Empty,
                        lector["EmailEscribano"].ToString() ?? string.Empty,
                        "", true, Rol.Escribano, null,
                        lector["NumCajaProf"].ToString() ?? string.Empty,
                        lector["DireccionEstudio"].ToString() ?? string.Empty);

                    resultado = new SolicitudEscribano(unaSolicitud, unEscribano);
                }

                lector.Close();
                return resultado;
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

        public List<SolicitudEscribano> ListarPorCliente(int pIdCliente)
        {
            List<SolicitudEscribano> lista = new List<SolicitudEscribano>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Notarial_ListarPorCliente", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdCliente", pIdCliente);

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while (lector.Read())
                {
                    Marcas unaMarca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(), unaMarca);

                    Cliente unVendedor = new Cliente(Convert.ToInt32(lector["IdVendedor"]),
                        lector["NombreVendedor"].ToString() ?? string.Empty,
                        lector["TelefonoVendedor"].ToString() ?? string.Empty,
                        lector["EmailVendedor"].ToString() ?? string.Empty,
                        "", true, Rol.Cliente, null, lector["CedulaVendedor"].ToString() ?? string.Empty);

                    decimal? latitud = lector["Latitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Latitud"]);
                    decimal? longitud = lector["Longitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Longitud"]);

                    Vehiculo unVehiculo = new Vehiculo(Convert.ToInt32(lector["IdVehiculo"]),
                        Convert.ToDecimal(lector["Precio"]),
                        Convert.ToInt32(lector["Kilometraje"]),
                        Convert.ToInt32(lector["Ano"]),
                        lector["CajaDeCambios"].ToString() ?? string.Empty,
                        lector["Motorizacion"].ToString() ?? string.Empty,
                        lector["Descripcion"].ToString() ?? string.Empty,
                        Convert.ToBoolean(lector["Publicado"]),
                        latitud,
                        longitud,
                        unModelo,
                        unVendedor,
                        new List<string>());

                    // Es "Mis Solicitudes" del propio cliente: no repetimos sus datos en cada fila.
                    Cliente unCliente = new Cliente(pIdCliente, string.Empty, string.Empty, string.Empty, "", true, Rol.Cliente, null, string.Empty);

                    SolicituNotarial unaSolicitud = new SolicituNotarial(
                        Convert.ToInt32(lector["IdSolicitud"]),
                        Convert.ToDateTime(lector["FechaSolicitud"]),
                        Convert.ToInt32(lector["EstadoSolicitud"]),
                        unCliente,
                        unVehiculo);

                    Escribano unEscribano = new Escribano(Convert.ToInt32(lector["IdEscribano"]),
                        lector["NombreEscribano"].ToString() ?? string.Empty,
                        lector["TelefonoEscribano"].ToString() ?? string.Empty,
                        lector["EmailEscribano"].ToString() ?? string.Empty,
                        "", true, Rol.Escribano, null,
                        lector["NumCajaProf"].ToString() ?? string.Empty,
                        lector["DireccionEstudio"].ToString() ?? string.Empty);

                    lista.Add(new SolicitudEscribano(unaSolicitud, unEscribano));
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

        public List<SolicitudEscribano> ListarPorEscribano(int pIdEscribano)
        {
            List<SolicitudEscribano> lista = new List<SolicitudEscribano>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Notarial_ListarPorEscribano", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@IdEscribano", pIdEscribano);

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while (lector.Read())
                {
                    Marcas unaMarca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(), unaMarca);

                    Cliente unVendedor = new Cliente(Convert.ToInt32(lector["IdVendedor"]),
                        lector["NombreVendedor"].ToString() ?? string.Empty,
                        lector["TelefonoVendedor"].ToString() ?? string.Empty,
                        lector["EmailVendedor"].ToString() ?? string.Empty,
                        "", true, Rol.Cliente, null, lector["CedulaVendedor"].ToString() ?? string.Empty);

                    decimal? latitud = lector["Latitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Latitud"]);
                    decimal? longitud = lector["Longitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Longitud"]);

                    Vehiculo unVehiculo = new Vehiculo(Convert.ToInt32(lector["IdVehiculo"]),
                        Convert.ToDecimal(lector["Precio"]),
                        Convert.ToInt32(lector["Kilometraje"]),
                        Convert.ToInt32(lector["Ano"]),
                        lector["CajaDeCambios"].ToString() ?? string.Empty,
                        lector["Motorizacion"].ToString() ?? string.Empty,
                        lector["Descripcion"].ToString() ?? string.Empty,
                        Convert.ToBoolean(lector["Publicado"]),
                        latitud,
                        longitud,
                        unModelo,
                        unVendedor,
                        new List<string>());

                    Cliente unCliente = new Cliente(Convert.ToInt32(lector["IdCliente"]),
                        lector["NombreCliente"].ToString() ?? string.Empty,
                        lector["TelefonoCliente"].ToString() ?? string.Empty,
                        lector["EmailCliente"].ToString() ?? string.Empty,
                        "", true, Rol.Cliente, null, lector["Cedula"].ToString() ?? string.Empty);

                    SolicituNotarial unaSolicitud = new SolicituNotarial(
                        Convert.ToInt32(lector["IdSolicitud"]),
                        Convert.ToDateTime(lector["FechaSolicitud"]),
                        Convert.ToInt32(lector["EstadoSolicitud"]),
                        unCliente,
                        unVehiculo);

                    // Es "Solicitudes Pendientes" del propio escribano: no repetimos sus datos en cada fila.
                    Escribano unEscribano = new Escribano(pIdEscribano, string.Empty, string.Empty, string.Empty, "", true, Rol.Escribano, null, string.Empty, string.Empty);

                    lista.Add(new SolicitudEscribano(unaSolicitud, unEscribano));
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
    }
}
