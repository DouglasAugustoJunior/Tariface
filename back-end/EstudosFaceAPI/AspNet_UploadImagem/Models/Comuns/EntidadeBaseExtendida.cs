using Newtonsoft.Json;

namespace AspNet_UploadImagem.Models
{
    public class EntidadeBaseExtendida: EntidadeBase
    {
        [JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }
    }
}
