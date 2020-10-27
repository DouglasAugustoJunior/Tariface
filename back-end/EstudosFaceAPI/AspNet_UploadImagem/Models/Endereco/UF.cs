using Newtonsoft.Json;
using System.Collections.Generic;

namespace AspNet_UploadImagem.Models
{
    public class UF: EntidadeBaseExtendida
    {
        [JsonProperty(PropertyName ="sigla")]
        public string Sigla { get; set; }

        [JsonProperty(PropertyName= "regiaoID")]
        public int RegiaoID { get; set; }

        [JsonProperty(PropertyName = "regiao")]
        public Regiao Regiao { get; set; }

        [JsonProperty(PropertyName = "municipios")]
        public IList<Municipio> Municipios { get; set; }
    }
}
