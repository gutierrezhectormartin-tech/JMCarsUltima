using System;
using System.Collections.Generic;
using System.Text;

namespace JMCarsApp.DTOs
{
    public class FiltrosVehiculoDTO
    {
        public List<ModeloDTO> Modelos { get; set; } = new List<ModeloDTO>();
        public List<int> Anios { get; set; } = new List<int>();

    }
}
