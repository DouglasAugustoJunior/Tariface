using Newtonsoft.Json;
using System.Collections.Generic;

namespace AspNet_UploadImagem.Models
{
    public class GrupoPessoa: EntidadeBaseExtendida
    {
        [JsonProperty(PropertyName = "idAzure")]
        public string IDAzure { get; set; }
        public IList<Usuario> Usuarios { get; set; }

        public GrupoPessoa() { }

        public GrupoPessoa(string idAzure, string nome)
        {
            IDAzure = idAzure;
            Nome    = nome;
        }
    }
}
