using Microsoft.AspNetCore.Mvc;
using AspNet_UploadImagem.Models;
using Microsoft.AspNetCore.Authorization;
using AspNet_UploadImagem.Handlers.EnderecoFolder;

namespace AspNet_UploadImagem.Controllers.EnderecoFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly EnderecoHandler _enderecoHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="enderecoHandler">Handler do Endereço</param>
        public EnderecoController(EnderecoHandler enderecoHandler)
        {
            _enderecoHandler = enderecoHandler;
        }

        /// <summary>
        /// Pega o endereço por ID
        /// </summary>
        /// <param name="id">ID do endereço a ser pesquisado</param>
        /// <returns>Objeto Endereço</returns>
        [Authorize]
        [HttpGet]
        [Route("{id:int}")]
        public Endereco PegaPorID([FromRoute] int id) => _enderecoHandler.PegaPorID(ID: id);

        /// <summary>
        /// Cria um novo endereço
        /// </summary>
        /// <param name="endereco">Endereço a ser cadastrado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("cadastrar")]
        public string CadastrarEndereco([FromBody] Endereco endereco) => _enderecoHandler.CriaEndereco(endereco: endereco);

        /// <summary>
        /// Atualiza o endereço
        /// </summary>
        /// <param name="endereco">Endereço a ser atualizado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("atualizar")]
        public string AtualizaEndereco([FromBody] Endereco endereco) => _enderecoHandler.AtualizaEndereco(endereco: endereco);

        /// <summary>
        /// Excluí um endereço
        /// </summary>
        /// <param name="idEndereco">ID do endereço a ser excluído</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("excluir")]
        public string Excluir([FromBody] Endereco endereco) => _enderecoHandler.ExcluiEndereco(endereco: endereco);
    }
}
