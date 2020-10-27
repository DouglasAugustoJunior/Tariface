using Newtonsoft.Json;
using System.Collections.Generic;

namespace AspNet_UploadImagem.Models
{
    public class StatusTransacao: EntidadeBaseExtendida
    {
        [JsonProperty(PropertyName = "historicos")]
        public IList<Historico> Historicos { get; set; }
    }
}
