using Microsoft.AspNetCore.Mvc;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using AspNet_UploadImagem.Handlers.EnderecoFolder;

namespace AspNet_UploadImagem.Controllers.EnderecoFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class UFController : ControllerBase
    {
        private readonly UFHandler _ufHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="ufHandler">Handler do UF</param>
        public UFController(UFHandler ufHandler)
        {
            _ufHandler = ufHandler;
        }

        /// <summary>
        /// Pega todos os UFs
        /// </summary>
        /// <returns>Lista de UFs</returns>
        [HttpGet]
        public IList<UF> PegaTodosUFs() => _ufHandler.PegaTodosUFs();
    }
}
