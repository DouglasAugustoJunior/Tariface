using Newtonsoft.Json;

namespace AspNet_UploadImagem.Models
{
    /// <summary>
    /// Usada para compor os atributos padrões de todos os modelos
    /// </summary>
    public class EntidadeBase
    {
        public EntidadeBase() { }

        [JsonProperty(PropertyName ="id")]
        public int ID { get; set; }

    }
}
