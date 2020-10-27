using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using Microsoft.AspNetCore.Authorization;
using AspNet_UploadImagem.Handlers.EnderecoFolder;

namespace AspNet_UploadImagem.Controllers.EnderecoFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipioController : ControllerBase
    {
        private readonly MunicipioHandler _municipioHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="municipioHandler">Handler dos municípios</param>
        public MunicipioController(MunicipioHandler municipioHandler)
        {
            _municipioHandler = municipioHandler;
        }

        /// <summary>
        /// Pega todos os Municípios
        /// </summary>
        /// <returns>Lista de Municípios</returns>
        [HttpGet]
        public IList<Municipio> PegaTodosMunicipios() => _municipioHandler.PegaTodosMunicipios();

        /// <summary>
        /// Pega os municípios pelo UF
        /// </summary>
        /// <param name="idUf">ID do UF</param>
        /// <returns>Lista de Municípios</returns>
        [HttpGet]
        [Route("filtroUF")]
        public IList<Municipio> PegaPorIDUF([FromQuery] int idUf) => _municipioHandler.PegaPorIDUF(idUF:idUf);
    }
}
