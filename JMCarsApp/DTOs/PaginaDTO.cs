using System;
using System.Collections.Generic;
using System.Text;

namespace JMCarsApp.DTOs
{
    public class PaginaDTO<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Total {get;set;}
        public int Pagina { get; set; }
        public int TamanoPagina { get; set; }
        public bool HayMas { get; set; }

    }
}
