using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Microsoft.Data.SqlClient;
using System.Data;
using Persistencia.Interfaces;

namespace Persistencia
{
    public class PersistenciaEstadistica : IPersistenciaEstadisticas
    {
        public Estadisticas ObtenerConteos()
        {
            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_Estadisticas_Conteos", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();
                Estadisticas estadisticas = new Estadisticas();

                if(lector.Read())
                {
                    estadisticas.CantPendientes = Convert.ToInt32(lector["CantPendientes"]);
                    estadisticas.CantAprobadas = Convert.ToInt32(lector["CantAprobadas"]);
                    estadisticas.CantRechazadas = Convert.ToInt32(lector["CantRechazadas"]);
                    estadisticas.CantInactivas = Convert.ToInt32(lector["CantInactivas"]);
                    estadisticas.CantEnTramite = Convert.ToInt32(lector["CantEnTramite"]);
                    estadisticas.CantVendidas = Convert.ToInt32(lector["CantVendidas"]);
                    estadisticas.CantClientesActivos = Convert.ToInt32(lector["CantClientesActivos"]);
                    estadisticas.CantEscribanosActivos = Convert.ToInt32(lector["CantEscribanosActivos"]);
                    estadisticas.CantCompraVentasFinalizadas = Convert.ToInt32(lector["CantCompraVentasFinalizadas"]);
                    estadisticas.CantSolicitudesPendientesVencidas = Convert.ToInt32(lector["CantSolicitudesPendientesVencidas"]);

                }
                return estadisticas;
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

        public List<CompraVentaPorMes> ObtenerComprasPorMes()
        {
            List<CompraVentaPorMes> lista = new List<CompraVentaPorMes>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_Estadisticas_ComprasPorMes", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while (lector.Read())
                {
                    CompraVentaPorMes cv = new CompraVentaPorMes();
                    cv.Anio = Convert.ToInt32(lector["Anio"]);
                    cv.Mes = Convert.ToInt32(lector["Mes"]);
                    cv.Cantidad = Convert.ToInt32(lector["Cantidad"]);

                    lista.Add(cv);
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

        public List<MarcaPublicada> ObtenerMarcasMasPublicadas()
        {
            List<MarcaPublicada> lista = new List<MarcaPublicada>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_Estadisticas_MarcasMasPublicadas", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while(lector.Read())
                {
                    MarcaPublicada mp = new MarcaPublicada();

                    mp.NombreMarca = lector["NombreMarca"].ToString();
                    mp.Cantidad = Convert.ToInt32(lector["Cantidad"]);

                    lista.Add(mp);
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

        public List<ModeloVendido> ObtenerModelosVendidosPorMarca()
        {
            List<ModeloVendido> lista = new List<ModeloVendido>();

            SqlConnection oConexion = new SqlConnection(Conexion.GetConexion());
            SqlCommand oComando = new SqlCommand("sp_Estadisticas_ModelosVendidosPorMarca", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();
                SqlDataReader lector = oComando.ExecuteReader();

                while(lector.Read())
                {
                    ModeloVendido mv = new ModeloVendido();

                    mv.NombreMarca = lector["NombreMarca"].ToString();
                    mv.NombreModelo = lector["NombreModelo"].ToString();
                    mv.Cantidad = Convert.ToInt32(lector["Cantidad"]);

                    lista.Add(mv);
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
    }
}
