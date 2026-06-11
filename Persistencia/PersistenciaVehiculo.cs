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

                SqlDataReader oReader =
                    oComando.ExecuteReader();

                while (oReader.Read())
                {
                    Marcas unaMarca = new Marcas(
                        Convert.ToInt32(oReader["IdMarca"]),
                        oReader["NombreMarca"].ToString()
                    );

                    Modelos unModelo = new Modelos(
                        Convert.ToInt32(oReader["IdModelo"]),
                        oReader["NombreModelo"].ToString(),
                        unaMarca
                    );

                    //Ubicaciones unaUbicacion = new Ubicaciones(
                    //    Convert.ToDecimal(oReader["Latitud"]),
                    //    Convert.ToDecimal(oReader["Longitud"])
                    //);

                    Cliente unCliente = new Cliente(
                        Convert.ToInt32(oReader["IdUsuario"]),
                        oReader["NombreCompleto"].ToString(),
                        "",
                        "",
                        "",
                        true,
                        ""
                    );


                    List<string> fotos = new List<string>();

                    Vehiculo unVehiculo = new Vehiculo(
                        Convert.ToInt32(oReader["IdVehiculo"]),
                        Convert.ToDecimal(oReader["Precio"]),
                        Convert.ToInt32(oReader["Kilometraje"]),
                        Convert.ToInt32(oReader["Ano"]),
                        oReader["CajaDeCambios"].ToString(),
                        oReader["Motorizacion"].ToString(),
                        oReader["Descripcion"].ToString(),
                        Convert.ToBoolean(oReader["Publicado"]),
                        //unaUbicacion,
                        unModelo,
                        unCliente,
                        fotos
                                        );

                    lista.Add(unVehiculo);
                }

                oReader.Close();

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