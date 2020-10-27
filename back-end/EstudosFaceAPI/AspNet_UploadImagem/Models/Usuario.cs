using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AspNet_UploadImagem.Models
{
    public class Usuario: EntidadeBaseExtendida
    {
        [JsonProperty(PropertyName = "cpf")]
        public string CPF { get; set; }

        [JsonProperty(PropertyName = "saldo")]
        public decimal Saldo { get; set; }

        [JsonProperty(PropertyName = "grupoPessoaId")]
        public int GrupoPessoaID { get; set; }

        [JsonProperty(PropertyName = "grupoPessoa")]
        public GrupoPessoa GrupoPessoa { get; set; }

        [JsonProperty(PropertyName = "personId")]
        public Guid PersonId { get; set; }

        [JsonProperty(PropertyName = "enderecoId")]
        public int EnderecoID { get; set; }

        [JsonProperty(PropertyName = "endereco")]
        public Endereco Endereco { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "senha")]
        public string Senha { get; set; }


        [JsonProperty(PropertyName = "imagens")]
        public IList<Imagem> Imagens { get; set;  }

        [JsonProperty(PropertyName = "cartoes")]
        public IList<Cartao> Cartoes { get; set; }

        [JsonProperty(PropertyName = "historico")]
        public IList<Historico> Historicos { get; set; }

        public Usuario() { }
        public Usuario(string cpf, string nome,int grupoPessoaId)
        {
            CPF           = cpf;
            Nome          = nome;
            GrupoPessoaID = grupoPessoaId;
        }
    }
}
