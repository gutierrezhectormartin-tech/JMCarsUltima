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
        private decimal? latitud;
        private decimal? longitud;
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
        
        public decimal? Latitud
        {
            get { return latitud; }
            set
            {
                latitud = value;
            }
        }

        public decimal? Longitud
        {
            get { return longitud; }
            set
            {
                longitud = value;
            }
        }
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
            bool pPublicado, decimal? pLatitud, decimal? pLongitud,
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
            Latitud = pLatitud;
            Longitud = pLongitud;
            Modelo = pModelo;
            Vendedor = pVendedor;
            Fotografia = pFotografia;
        }
        public Vehiculo() {}

        public void Validar()
        {
            if (Precio <= 0)
            {
                throw new Exception("El precio debe ser un valor mayor a cero.");
            }

            if (Km < 0)
            {
                throw new Exception("El kilometraje no puede ser negativo.");
            }

            if (Anio <= 1900 || Anio > DateTime.Now.Year + 1)
            {
                throw new Exception("El año ingresado no es válido.");
            }

            if (string.IsNullOrWhiteSpace(CajaCambios))
            {
                throw new Exception("Debe especificar la caja de cambios.");
            }

            if (string.IsNullOrWhiteSpace(Motorizacion))
            {
                throw new Exception("Debe especificar la motorización.");
            }

            if (Modelo == null || Modelo.IdModelo <= 0)
            {
                throw new Exception("El vehículo debe tener un modelo asociado.");
            }

            if (Vendedor == null || Vendedor.IdUsuario <= 0)
            {
                throw new Exception("El vehículo debe tener un vendedor asociado.");
            }
        }
    }
}
