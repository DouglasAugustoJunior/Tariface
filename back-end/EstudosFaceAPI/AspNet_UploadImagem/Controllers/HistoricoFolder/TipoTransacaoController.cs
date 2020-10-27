using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using Microsoft.AspNetCore.Authorization;
using AspNet_UploadImagem.Handlers.HistoricoFolder;

namespace AspNet_UploadImagem.Controllers.HistoricoFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoTransacaoController : ControllerBase
    {
        private readonly TipoTransacaoHandler _tipoTransacaoHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="tipoTransacaoHandler">Handler dos Tipos</param>
        public TipoTransacaoController(TipoTransacaoHandler tipoTransacaoHandler)
        {
            _tipoTransacaoHandler = tipoTransacaoHandler;
        }

        /// <summary>
        /// Pega todos os Tipos
        /// </summary>
        /// <returns>Lista de Tipos</returns>
        [Authorize]
        [HttpGet]
        public IList<TipoTransacao> PegaTodosTipos() => _tipoTransacaoHandler.PegaTodosTipos();
    }
}
