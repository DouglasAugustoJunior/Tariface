using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace AspNet_UploadImagem.Models
{
    public class Endereco : EntidadeBase
    {
        [JsonProperty(PropertyName = "logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty(PropertyName = "numero")]
        public int Numero { get; set; }

        [JsonProperty(PropertyName = "cep")]
        public int CEP { get; set; }
        
        [JsonProperty(PropertyName = "municipioId")]
        public int MunicipioID { get; set; }

        [JsonProperty(PropertyName = "municipio")]
        public Municipio Municipio { get; set; }

        [JsonProperty(PropertyName = "complemento")]
        public string Complemento { get; set; }

        [JsonProperty(PropertyName = "usuarios")]
        public IList<Usuario> Usuarios { get; set; }
    }
}
