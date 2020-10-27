using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using AspNet_UploadImagem.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace AspNet_UploadImagem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {
        private readonly CartaoHandler _cartaoHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="cartaoHandler">Handler do cartão</param>
        public CartaoController(CartaoHandler cartaoHandler)
        {
            _cartaoHandler = cartaoHandler;
        }

        /// <summary>
        /// Pega todos os cartões por ID de usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário a ser pesquisado</param>
        /// <returns>Lista de cartões do usuário</returns>
        [Authorize]
        [HttpGet]
        [Route("{usuarioId:int}")]
        public IList<Cartao> PegaPorIDUsuario([FromRoute] int usuarioId) => _cartaoHandler.PegaPorIDUsuario(usuarioId: usuarioId);

        /// <summary>
        /// Cria um novo cartão
        /// </summary>
        /// <param name="cartao">Cartão a ser cadastrado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("cadastrar")]
        public string CadastrarCartao([FromBody] Cartao cartao) => _cartaoHandler.CriaCartao(cartao: cartao);

        /// <summary>
        /// Atualiza o cartão
        /// </summary>
        /// <param name="cartao">Cartão a ser atualizado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("atualizar")]
        public string AtualizaCartao([FromBody] Cartao cartao) => _cartaoHandler.AtualizaCartao(cartao: cartao);

        /// <summary>
        /// Excluí um cartão
        /// </summary>
        /// <param name="idCartao">ID do cartão a ser excluído</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("excluir")]
        public string Excluir([FromQuery] int id) => _cartaoHandler.ExcluiCartao(id: id);
    }
}
