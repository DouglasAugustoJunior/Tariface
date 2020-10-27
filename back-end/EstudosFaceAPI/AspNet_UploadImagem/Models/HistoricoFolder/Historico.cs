using Newtonsoft.Json;
using System;

namespace AspNet_UploadImagem.Models
{
    public class Historico: EntidadeBase
    {
        [JsonProperty(PropertyName = "idUsuario")]
        public int IDUsuario { get; set; }

        [JsonProperty(PropertyName = "usuario")]
        public Usuario Usuario { get; set; }

        [JsonProperty(PropertyName = "valor")]
        public decimal Valor { get; set; }

        [JsonProperty(PropertyName = "dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty(PropertyName = "tipoId")]
        public int TipoID { get; set; }

        [JsonProperty(PropertyName = "tipo")]
        public TipoTransacao TipoTransacao { get; set; }

        [JsonProperty(PropertyName = "statusId")]
        public int StatusID { get; set; }

        [JsonProperty(PropertyName = "status")]
        public StatusTransacao StatusTransacao { get; set; }

        public Historico()
        {
            DataCriacao = DateTime.Now;
        }
    }
}
