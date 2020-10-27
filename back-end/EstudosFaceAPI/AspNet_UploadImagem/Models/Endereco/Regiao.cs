using Newtonsoft.Json;
using System.Collections.Generic;

namespace AspNet_UploadImagem.Models
{
    public class Regiao: EntidadeBaseExtendida
    {
        [JsonProperty(PropertyName = "ufs")]
        public IList<UF> UFs { get; set; }
    }
}
