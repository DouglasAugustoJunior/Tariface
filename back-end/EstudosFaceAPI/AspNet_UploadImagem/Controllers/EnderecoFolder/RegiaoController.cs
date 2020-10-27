using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using Microsoft.AspNetCore.Authorization;
using AspNet_UploadImagem.Handlers.EnderecoFolder;

namespace AspNet_UploadImagem.Controllers.EnderecoFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegiaoController : ControllerBase
    {
        private readonly RegiaoHandler _regiaoHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="regiaoHandler">Handler da região</param>
        public RegiaoController(RegiaoHandler regiaoHandler)
        {
            _regiaoHandler = regiaoHandler;
        }

        /// <summary>
        /// Pega todas as regiões
        /// </summary>
        /// <returns>Lista de regiões</returns>
        [Authorize]
        [HttpGet]
        public IList<Regiao> PegaTodasRegioes() => _regiaoHandler.PegaTodasRegioes();

    }
}
