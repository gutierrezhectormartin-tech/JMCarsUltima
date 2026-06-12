using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Modelos
    {
        private int idModelo;
        private string modelo;
        private Marcas marca;

        public int IdModelo
        {
            get { return idModelo; }
            set
            {
                idModelo = value;
            }
        }

        public string Modelo
        {
            get { return modelo; }
            set
            {
                modelo = value;
            }
        }

        public Marcas Marca
        {
            get { return marca; }
            set
            {
                marca = value;
            }
        }

        public Modelos(int pIdModelo, string pModelo, Marcas pMarca)
        {
            IdModelo = pIdModelo;
            Modelo = pModelo;
            Marca = pMarca;
        }

        public Modelos() { }

    }
}
