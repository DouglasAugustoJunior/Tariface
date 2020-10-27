using Microsoft.AspNetCore.Mvc;
using AspNet_UploadImagem.Models;
using AspNet_UploadImagem.Handlers;

namespace AspNet_UploadImagem.Controllers.AutenticacaoJWT
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Constantes
        private readonly LoginHandler _loginHandler;
        #endregion Constantes

        public LoginController(LoginHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            var resultado = _loginHandler.Login(login: login);
            if (resultado != null) return Ok(resultado);
            else                   return Unauthorized();
        }
        
    }
}
