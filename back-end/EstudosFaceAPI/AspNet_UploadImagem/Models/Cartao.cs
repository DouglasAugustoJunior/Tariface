using System;
using Newtonsoft.Json;

namespace AspNet_UploadImagem.Models
{
    public class Cartao: EntidadeBase
    {
        [JsonProperty(PropertyName = "numero")]
        public string Numero { get; set; }

        [JsonProperty(PropertyName = "titular")]
        public string Titular { get; set; }

        [JsonProperty(PropertyName = "validade")]
        public DateTime Validade { get; set; }

        [JsonProperty(PropertyName = "csv")]
        public int CSV { get; set; }

        [JsonProperty(PropertyName = "idUsuario")]
        public int IDUsuario { get; set; }

        [JsonProperty(PropertyName = "usuario")]
        public Usuario Usuario { get; set; }
    }
}
