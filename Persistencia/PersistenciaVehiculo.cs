using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Modelo;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

namespace Persistencia
{
    public class PersistenciaVehiculo : IPersistenciaVehiculo
    {
        public List<Vehiculo> ListarVehiculos()
        {
            List<Vehiculo> lista = new List<Vehiculo>();

            SqlConnection oConexion =
                new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando =
                new SqlCommand("sp_Vehiculo_Listar", oConexion);

            oComando.CommandType =
                CommandType.StoredProcedure;

            try
            {
                oConexion.Open();

                SqlDataReader lector =
                    oComando.ExecuteReader();

                while (lector.Read())
                {
                    Marcas unaMarca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(), unaMarca);

                    Cliente unCliente = new Cliente(Convert.ToInt32(lector["IdUsuario"]), lector["NombreCompleto"].ToString() ?? string.Empty,
                        "", "", "", true, Rol.Cliente, null, "");


                    List<string> fotos = new List<string>();
                    decimal? latitud = lector["Latitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Latitud"]);
                    decimal? longitud = lector["Longitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Longitud"]);

                    Vehiculo unVehiculo = new Vehiculo(Convert.ToInt32(lector["IdVehiculo"]),
                        Convert.ToDecimal(lector["Precio"]),
                        Convert.ToInt32(lector["Kilometraje"]),
                        Convert.ToInt32(lector["Ano"]),
                        lector["CajaDeCambios"].ToString() ??string.Empty,
                        lector["Motorizacion"].ToString() ?? string.Empty,
                        lector["Descripcion"].ToString() ?? string.Empty,
                        Convert.ToBoolean(lector["Publicado"]),
                        latitud,
                        longitud,
                        unModelo,
                        unCliente,
                        fotos);

                    lista.Add(unVehiculo);
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
        public List<Vehiculo> ListarMisVehiculos(string idUsuario)
        {
            List<Vehiculo> lista = new List<Vehiculo>();

            SqlConnection oConexion =
                new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando =
                new SqlCommand("sp_Vehiculo_ListarMisVehiculos", oConexion);

            oComando.CommandType =
                CommandType.StoredProcedure;

            try
            {
                oComando.Parameters.AddWithValue("@IdUsuario", Convert.ToInt32(idUsuario));

                oConexion.Open();

                SqlDataReader lector = oComando.ExecuteReader();
               

                while (lector.Read())
                {
                    Marcas unaMarca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(),
                        unaMarca);

                    Cliente unCliente = new Cliente(Convert.ToInt32(lector["IdUsuario"]), lector["NombreCompleto"].ToString() ?? string.Empty,
                        "", "", "", true, Rol.Cliente, null, "");


                    List<string> fotos = new List<string>();
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
                        unCliente,
                        fotos);

                    lista.Add(unVehiculo);
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

        public List<Vehiculo> BuscarGeneral(decimal pLatCli, decimal pLonCli, int pRadioKm, int? pIdMarca, decimal? pPrecioMax)
        {
            List<Vehiculo> listaV = new List<Vehiculo>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Vehiculo_BuscarGeneral", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@LatCli", pLatCli);
            oComando.Parameters.AddWithValue("@LonCli", pLonCli);
            oComando.Parameters.AddWithValue("@RadioKM", pRadioKm);
            oComando.Parameters.AddWithValue("@IdMarca", (object)pIdMarca ?? DBNull.Value);
            oComando.Parameters.AddWithValue("@PrecioMax", (object)pPrecioMax ?? DBNull.Value);

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while(lector.Read())
                {
                    Marcas unaMarca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(), unaMarca);

                    Cliente unCliente = new Cliente(Convert.ToInt32(lector["IdUsuario"]), lector["NombreCompleto"].ToString(), "", "", "", true, Rol.Cliente, null, "");

                    decimal? latitud = lector["Latitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Latitud"]);
                    decimal? longitud = lector["Longitud"] == DBNull.Value ? null : Convert.ToDecimal(lector["Longitud"]);

                    List<string> fotos = new List<String>();

                    Vehiculo unVehiculo = new Vehiculo(Convert.ToInt32(lector["IdVehiculo"]), Convert.ToDecimal(lector["Precio"]),
                                                        Convert.ToInt32(lector["Kilometraje"]), Convert.ToInt32(lector["Ano"]),
                                                        lector["CajaDeCambios"].ToString(), lector["Motorizacion"].ToString(),
                                                        lector["Descripcion"].ToString(), Convert.ToBoolean(lector["Publicado"]),
                                                        latitud, longitud, unModelo, unCliente, fotos);
                    listaV.Add(unVehiculo);
                }
                return listaV;
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

        public void Registrar(Vehiculo pVehiculo)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());

            SqlCommand oComando = new SqlCommand("sp_Vehiculo_Crear", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            // --- PRECIO: Especificamos DECIMAL(10,2) ---
            SqlParameter paramPrecio = new SqlParameter("@Precio", SqlDbType.Decimal);
            paramPrecio.Precision = 10;
            paramPrecio.Scale = 2;
            paramPrecio.Value = pVehiculo.Precio;
            oComando.Parameters.Add(paramPrecio);

            oComando.Parameters.AddWithValue("@Kilometraje", pVehiculo.Km);
            oComando.Parameters.AddWithValue("@Ano", pVehiculo.Anio);
            oComando.Parameters.AddWithValue("@Caja", pVehiculo.CajaCambios);
            oComando.Parameters.AddWithValue("@Motor", pVehiculo.Motorizacion);
            oComando.Parameters.AddWithValue("@Desc", pVehiculo.Descripcion ?? (object)DBNull.Value);

            // --- NUEVO: Mandamos los textos libres para que el SP resuelva la lógica ---
            oComando.Parameters.AddWithValue("@NombreMarca", pVehiculo.Modelo.Marca.NombreMarca);
            oComando.Parameters.AddWithValue("@NombreModelo", pVehiculo.Modelo.Modelo);
            oComando.Parameters.AddWithValue("@IdVendedor", pVehiculo.Vendedor.IdUsuario);

            // --- LATITUD: Especificamos DECIMAL(9,6) ---
            SqlParameter paramLat = new SqlParameter("@Lat", SqlDbType.Decimal);
            paramLat.Precision = 9;
            paramLat.Scale = 6;
            if (pVehiculo.Latitud.HasValue)
                paramLat.Value = pVehiculo.Latitud.Value;
            else
                paramLat.Value = DBNull.Value;
            oComando.Parameters.Add(paramLat);

            // --- LONGITUD: Especificamos DECIMAL(9,6) ---
            SqlParameter paramLon = new SqlParameter("@Lon", SqlDbType.Decimal);
            paramLon.Precision = 9;
            paramLon.Scale = 6;
            if (pVehiculo.Longitud.HasValue)
                paramLon.Value = pVehiculo.Longitud.Value;
            else
                paramLon.Value = DBNull.Value;
            oComando.Parameters.Add(paramLon);

            try
            {
                oConexion.Open();

                object resultado = oComando.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    int idVehiculoGenerado = Convert.ToInt32(resultado);
                    pVehiculo.IdVehiculo = Convert.ToInt32(resultado);
                    if (pVehiculo.Fotografia != null && pVehiculo.Fotografia.Count > 0)
                    {
                        foreach (string url in pVehiculo.Fotografia)
                        {
                            SqlCommand cmdFoto = new SqlCommand("INSERT INTO FotoVehiculo (UrlFoto, IdVehiculo) VALUES (@Url, @Id)", oConexion);
                            cmdFoto.Parameters.AddWithValue("@Url", url);
                            cmdFoto.Parameters.AddWithValue("@Id", idVehiculoGenerado);
                            cmdFoto.ExecuteNonQuery();
                        }
                    }
                }
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
   
