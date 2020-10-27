using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNet_UploadImagem.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace AspNet_UploadImagem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagemController : ControllerBase
    {
        private readonly ImagemHandler _imagemHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="imagemHandler">Handler da imagem</param>
        public ImagemController(ImagemHandler imagemHandler)
        {
            _imagemHandler = imagemHandler;
        }

        /// <summary>
        /// Rota inicial
        /// </summary>
        /// <returns>String com dados da execução</returns>
        [HttpGet]
        public string Get() => _imagemHandler.InformacoesExecucao();

        /// <summary>
        /// Cria o grupo de pessoas que vai receber os persons dos usuários
        /// </summary>
        /// <param name="grupoPessoas">grupo de pessoas a ser criado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("criaGrupoPessoas")]
        public Task<string> CriaGrupoPessoas([FromQuery] string grupoPessoas) => _imagemHandler.CriaGrupoPessoasAsync(grupoPessoas: grupoPessoas);

        /// <summary>
        /// Rota de reconhecimento facial
        /// </summary>
        /// <param name="arquivo">Arquivo a ser salvo</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("reconhecimentoFacial")]
        public Task<string> ReconhecimentoFacial([FromForm] IFormFile arquivo) => _imagemHandler.ReconhecimentoFacialAsync(arquivo: arquivo);

        /// <summary>
        /// Rota de Upload das imagens
        /// </summary>
        /// <param name="arquivo">Arquivo a ser salvo</param>
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("upload")]
        public Task<string> EnviaArquivo([FromForm] IFormFile arquivo,[FromQuery]int idUsuario) => _imagemHandler.CadastraImagemAsync(arquivo: arquivo, idUsuario: idUsuario);

        /// <summary>
        /// Rota de Upload das imagens de perfil
        /// </summary>
        /// <param name="arquivo">Arquivo a ser salvo</param>
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns></returns>
        [HttpPost("uploadImagemPerfil")]
        public Task<string> EnviaImagemPerfil([FromForm] IFormFile arquivo, [FromQuery] int idUsuario) => _imagemHandler.CadastraImagemPerfilAsync(arquivo: arquivo, idUsuario: idUsuario);

        /// <summary>
        /// Cria o PersonGroupPerson que vai receber os rostos dos usuários
        /// </summary>
        /// <param name="idUsuario">ID do usuário que vai ter um PersonGroupPerson</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("criaPersonGroupPerson")]
        public Task<bool> CriaPersonGroupPerson([FromQuery] int idUsuario) => _imagemHandler.CriaPersonGroupPersonAsync(idUsuario: idUsuario);

        /// <summary>
        /// Realiza o cadastro dos rostos do usuário
        /// </summary>
        /// <param name="idUsuario">ID do usuário que terá os rostos cadastrados</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("cadastraRostos")]
        public Task<string> CadastraRostos([FromQuery] int idUsuario) => _imagemHandler.CadastraRostosEndPointAsync(idUsuario: idUsuario);

        /// <summary>
        /// Realiza treinamento com rostos do usuário
        /// </summary>
        /// <param name="grupoPessoasIDAzure">ID na Azure do grupo de pessoas a ser treinado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("treinar")]
        public Task<bool> Treinar([FromQuery] string grupoPessoasIDAzure) => _imagemHandler.TreinaComImagensUsuarioAsync(grupoPessoasIDAzure: grupoPessoasIDAzure);

        #region Exclusões

        /// <summary>
        /// Exclui o grupo de pessoas do Azure
        /// </summary>
        /// <param name="grupoPessoas">Nome do grupo de pessoas</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("excluiGrupoPessoas")]
        public Task<string> ExcluiGrupoPessoas([FromQuery] string grupoPessoas) => _imagemHandler.ExcluiGrupoPessoas(grupoPessoas: grupoPessoas);

        /// <summary>
        /// Exclui o PersonGroupPerson do usuário do Azure
        /// </summary>
        /// <param name="grupoPessoas">Nome do grupo de pessoas</param>
        /// <param name="idUsuario">ID do usuário</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("excluiPersonGroupPerson")]
        public Task<string> ExcluiPersonGroupPerson([FromQuery] string grupoPessoas,[FromQuery] int idUsuario) => _imagemHandler.ExcluiPersonGroupPerson(grupoPessoas: grupoPessoas,idUsuario: idUsuario);

        #endregion Exclusões
    }
}