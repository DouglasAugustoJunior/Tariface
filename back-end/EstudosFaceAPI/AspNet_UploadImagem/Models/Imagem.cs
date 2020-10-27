using Newtonsoft.Json;

namespace AspNet_UploadImagem.Models
{
    public class Imagem: EntidadeBaseExtendida
    {
        [JsonProperty(PropertyName = "idUsuario")]
        public int IDUsuario { get; set; }

        [JsonProperty(PropertyName = "usuario")]
        public Usuario Usuario { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }

        [JsonProperty(PropertyName = "persistedFaceId")]
        public int? PersistedFaceID { get; set; }

        [JsonProperty(PropertyName = "perfil")]
        public bool? Perfil { get; set; }

        public Imagem() { }

        public Imagem(int idUsuario, string url, string nome)
        {
            IDUsuario  = idUsuario;
            URL        = url;
            Nome       = nome;
        }
    }
}
