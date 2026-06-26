using Microsoft.Data.SqlClient;
using Modelo;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

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

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(), 
                        unaMarca);

                    //Ubicaciones unaUbicacion = new Ubicaciones(
                    //    Convert.ToDecimal(oReader["Latitud"]),
                    //    Convert.ToDecimal(oReader["Longitud"])
                    //);

                    Cliente unCliente = new Cliente(Convert.ToInt32(lector["IdUsuario"]), lector["NombreCompleto"].ToString() ?? string.Empty,
                        "", "", "", true, Rol.Cliente, null, "");


                    List<string> fotos = new List<string>();

                    Vehiculo unVehiculo = new Vehiculo(Convert.ToInt32(lector["IdVehiculo"]),
                        Convert.ToDecimal(lector["Precio"]),
                        Convert.ToInt32(lector["Kilometraje"]),
                        Convert.ToInt32(lector["Ano"]),
                        lector["CajaDeCambios"].ToString() ??string.Empty,
                        lector["Motorizacion"].ToString() ?? string.Empty,
                        lector["Descripcion"].ToString() ?? string.Empty,
                        Convert.ToBoolean(lector["Publicado"]),
                        //unaUbicacion,
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
                oConexion.Open();

                SqlDataReader lector = oComando.ExecuteReader();
                oComando.Parameters.AddWithValue("@IdUsuario", Convert.ToInt32(idUsuario));

                while (lector.Read())
                {
                    Marcas unaMarca = new Marcas(Convert.ToInt32(lector["IdMarca"]), lector["NombreMarca"].ToString());

                    Modelos unModelo = new Modelos(Convert.ToInt32(lector["IdModelo"]), lector["NombreModelo"].ToString(),
                        unaMarca);

                    //Ubicaciones unaUbicacion = new Ubicaciones(
                    //    Convert.ToDecimal(oReader["Latitud"]),
                    //    Convert.ToDecimal(oReader["Longitud"])
                    //);

                    Cliente unCliente = new Cliente(Convert.ToInt32(lector["IdUsuario"]), lector["NombreCompleto"].ToString() ?? string.Empty,
                        "", "", "", true, Rol.Cliente, null, "");


                    List<string> fotos = new List<string>();

                    Vehiculo unVehiculo = new Vehiculo(Convert.ToInt32(lector["IdVehiculo"]),
                        Convert.ToDecimal(lector["Precio"]),
                        Convert.ToInt32(lector["Kilometraje"]),
                        Convert.ToInt32(lector["Ano"]),
                        lector["CajaDeCambios"].ToString() ?? string.Empty,
                        lector["Motorizacion"].ToString() ?? string.Empty,
                        lector["Descripcion"].ToString() ?? string.Empty,
                        Convert.ToBoolean(lector["Publicado"]),
                        //unaUbicacion,
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
    }
}
   
