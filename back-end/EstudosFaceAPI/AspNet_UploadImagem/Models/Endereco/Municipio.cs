using Newtonsoft.Json;
using System.Collections.Generic;

namespace AspNet_UploadImagem.Models
{
    public class Municipio: EntidadeBaseExtendida
    {
        [JsonProperty(PropertyName = "ufId")]
        public int UfID { get; set; }

        [JsonProperty(PropertyName = "uf")]
        public UF UF { get; set; }

        [JsonProperty(PropertyName = "enderecos")]
        public IList<Endereco> Enderecos { get; set; }
    }
}
