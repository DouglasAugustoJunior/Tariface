using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using Microsoft.AspNetCore.Authorization;
using AspNet_UploadImagem.Handlers.HistoricoFolder;

namespace AspNet_UploadImagem.Controllers.HistoricoFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusTransacaoController : ControllerBase
    {
        private readonly StatusTransacaoHandler _statusTransacaoHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="statusTransacaoHandler">Handler dos Status</param>
        public StatusTransacaoController(StatusTransacaoHandler statusTransacaoHandler)
        {
            _statusTransacaoHandler = statusTransacaoHandler;
        }

        /// <summary>
        /// Pega todos os Status
        /// </summary>
        /// <returns>Lista de Status</returns>
        [Authorize]
        [HttpGet]
        public IList<StatusTransacao> PegaTodosStatus() => _statusTransacaoHandler.PegaTodosStatus();
    }
}
