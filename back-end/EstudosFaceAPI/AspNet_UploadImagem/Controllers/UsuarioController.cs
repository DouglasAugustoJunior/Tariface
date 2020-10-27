using Microsoft.AspNetCore.Mvc;
using AspNet_UploadImagem.Models;
using AspNet_UploadImagem.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace AspNet_UploadImagem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioHandler _usuarioHandler;

        /// <summary>
        /// Construtor injetando dependências
        /// </summary>
        /// <param name="usuarioHandler">Handler do usuário</param>
        public UsuarioController(UsuarioHandler usuarioHandler)
        {
            _usuarioHandler = usuarioHandler;
        }

        /// <summary>
        /// Pega o usuário pelo ID
        /// </summary>
        /// <param name="idUsuario">ID do usuário buscado</param>
        /// <returns>Usuário</returns>
        [Authorize]
        [HttpGet("pegaUsuarioPorID")]
        public Usuario PegaUsuarioPorID([FromQuery] int idUsuario) => _usuarioHandler.PegaPorID(ID: idUsuario);

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="usuario">Usuário a ser criado</param>
        /// <returns></returns>
        [HttpPost("cadastrarUsuario")]
        public int Cadastrar([FromBody] Usuario usuario) => _usuarioHandler.CriaUsuario(usuario: usuario);

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        /// <param name="usuario">Usuário a ser açterado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("atualizarUsuario")]
        public Usuario Atualizar([FromBody] Usuario usuario) => _usuarioHandler.AtualizarUsuario(usuario: usuario);

        /// <summary>
        /// Adiciona crédito ao usuário
        /// </summary>
        /// <param name="idUsuario">id do usuário</param>
        /// <param name="valor">valor a ser adicionado</param>
        /// <returns>Mensagem de sucesso ou falha</returns>
        [Authorize]
        [HttpPost("adicionaSaldo")]
        public string AdicionaSaldo([FromQuery] int idUsuario,[FromQuery] decimal valor) => _usuarioHandler.AdicionaSaldo(idUsuario: idUsuario, valor: valor);
    }
}
