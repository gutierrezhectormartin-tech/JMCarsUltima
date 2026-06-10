using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Modelo
{
    public class Ubicaciones
    {
        public class Ubicacion
        {
            // Coordenadas y datos principales
            [JsonPropertyName("place_id")]
            public long PlaceId { get; set; }

            [JsonPropertyName("licence")]
            public string Licencia { get; set; }

            [JsonPropertyName("osm_type")]
            public string OsmType { get; set; }

            [JsonPropertyName("osm_id")]
            public long OsmId { get; set; }

            // Nominatim devuelve lat/lon como strings en su JSON raíz
            [JsonPropertyName("lat")]
            public string LatitudString { get; set; }

            [JsonPropertyName("lon")]
            public string LongitudString { get; set; }

            // Propiedades calculadas para facilitar el uso numérico
            public double Latitud => double.TryParse(LatitudString, out var lat) ? lat : 0;
            public double Longitud => double.TryParse(LongitudString, out var lon) ? lon : 0;

            [JsonPropertyName("display_name")]
            public string DireccionCompleta { get; set; }

            // Contenedor de la dirección desglosada
            [JsonPropertyName("address")]
            public ComponentesDireccion DireccionDetallada { get; set; }
        }

        public class ComponentesDireccion
        {
            [JsonPropertyName("road")]
            public string Calle { get; set; }

            [JsonPropertyName("house_number")]
            public string Numero { get; set; }

            [JsonPropertyName("suburb")]
            public string? Barrio { get; set; }

            [JsonPropertyName("city")]
            public string? Ciudad { get; set; }

            [JsonPropertyName("county")]
            public string? Municipio { get; set; }

            [JsonPropertyName("state")]
            public string? Estado { get; set; }

            [JsonPropertyName("postcode")]
            public string? CodigoPostal { get; set; }

            [JsonPropertyName("country")]
            public string Pais { get; set; }

            [JsonPropertyName("country_code")]
            public string CodigoPais { get; set; }
        }
    }
}
