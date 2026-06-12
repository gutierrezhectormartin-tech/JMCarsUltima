using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Marcas
    {
        private int idMarca;
        private string nombreMarca;

        public int IdMarca
        {
            get { return idMarca; }
            set
            {
                idMarca = value;
            }
        }

        public string NombreMarca
        {
            get { return nombreMarca; }
            set
            {
                nombreMarca = value;
            }
        }

        public Marcas(int pIdMarca, string pNombreMarca)
        {
            IdMarca = pIdMarca;
            NombreMarca = pNombreMarca;
        }

        public Marcas() { }
    }
}
