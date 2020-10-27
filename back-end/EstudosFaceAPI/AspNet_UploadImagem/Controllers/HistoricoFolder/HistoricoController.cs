using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using Microsoft.AspNetCore.Authorization;
using AspNet_UploadImagem.Handlers.HistoricoFolder;

namespace AspNet_UploadImagem.Controllers.HistoricoFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoController : ControllerBase
    {
        private readonly HistoricoHandler _historicoHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="historicoHandler">Handler do histórico</param>
        public HistoricoController(HistoricoHandler historicoHandler)
        {
            _historicoHandler = historicoHandler;
        }

        /// <summary>
        /// Pega todos os históricos por ID de usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário a ser pesquisado</param>
        /// <returns>Lista de histórico do usuário</returns>
        [Authorize]
        [HttpGet]
        [Route("{cpf:int}")]
        public IList<Historico> PegaPorUsuarioID([FromRoute] int usuarioId) => _historicoHandler.PegaPorUsuarioID(usuarioId: usuarioId);
    }
}
