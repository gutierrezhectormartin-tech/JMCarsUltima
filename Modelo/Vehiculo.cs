using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Vehiculo
    {
        private int idVehiculo;
        private decimal precio;
        private int km;
        private int anio;
        private string cajaCambios;
        private string motorizacion;
        private string descripcion;
        private bool publicado;
        //private Ubicaciones ubicacion;
        private Modelos modelo;
        private Cliente vendedor;
        private List<string> fotografia;

        public int IdVehiculo
        {
            get { return idVehiculo; }
            set
            {
                idVehiculo = value;
            }
        }
        public decimal Precio
        {
            get { return precio; }
            set
            {
                precio = value;
            }
        }
        public int Km
        {
            get { return km; }
            set
            {
                km = value;
            }
        }
        public int Anio
        {
            get { return anio; }
            set
            {
                anio = value;
            }
        }
        public string CajaCambios
        {
            get { return cajaCambios; }
            set
            {
                cajaCambios = value;
            }
        }
        public string Motorizacion
        {
            get { return motorizacion; }
            set
            {
                motorizacion = value;
            }
        }
        public string Descripcion
        { 
            get { return descripcion; }
            set
            {
                descripcion = value;
            }
        }
        public bool Publicado
        {
            get { return publicado; }
            set
            {
                publicado = value;
            }
        }
        //public Ubicaciones Ubicacion
        //{
        //    get { return ubicacion; }
        //    set
        //    {
        //        ubicacion = value;
        //    }
        //}
        public Modelos Modelo
        {
            get { return modelo; }
            set
            {
                modelo = value;
            }
        }
        public Cliente Vendedor
        {
            get { return vendedor; }
            set
            {
                vendedor = value;
            }
        }

        public List<string> Fotografia
        {
            get { return fotografia; }
            set
            {
                fotografia = value;
            }
        }

        public Vehiculo(int pIdVehiculo,
            decimal pPrecio, int pKm, int pAnio,
            string pCaja, string pMotorizacion,
            string pDescripcion,
            bool pPublicado, /*Ubicaciones pUbicacion,*/
            Modelos pModelo, Cliente pVendedor, List<string> pFotografia)
        {
            IdVehiculo = pIdVehiculo;
            Precio = pPrecio;
            Km = pKm;
            Anio = pAnio;
            CajaCambios = pCaja;
            Motorizacion = pMotorizacion;
            Descripcion = pDescripcion;
            Publicado = pPublicado;
            //Ubicacion = pUbicacion;
            Modelo = pModelo;
            Vendedor = pVendedor;
            Fotografia = pFotografia;
        }
    }
}
