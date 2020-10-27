using Newtonsoft.Json;

namespace AspNet_UploadImagem.Models
{
    public class Login
    {
        [JsonProperty(PropertyName ="email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "senha")]
        public string Senha { get; set; }
    }
}
