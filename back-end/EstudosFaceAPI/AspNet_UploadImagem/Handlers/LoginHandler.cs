using System;
using System.Text;
using AspNet_UploadImagem.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using AspNet_UploadImagem.Models.AutenticacaoJWT;

namespace AspNet_UploadImagem.Handlers
{
    public class LoginHandler
    {
        #region Constantes
        private readonly UsuarioHandler _usuarioHandler;
        private readonly IConfiguration _configuracao;
        #endregion Constantes

        /// <summary>
        /// Injeção de dependências
        /// </summary>
        /// <param name="usuarioHandler">Dependência para recuperar o usuário</param>
        public LoginHandler(IConfiguration configuracao, UsuarioHandler usuarioHandler) {
            _configuracao   = configuracao;
            _usuarioHandler = usuarioHandler;
        }

        /// <summary>
        /// Retorna o JWT ou nulo dependendo do usuário e senha
        /// </summary>
        /// <param name="login">Usuário e senha</param>
        /// <returns>String JWT ou nulo</returns>
        internal RetornoAutenticacao Login(Login login)
        {
            Usuario resultado = ValidarUsuario(login: login);
            if (resultado != null)
            {
                RetornoAutenticacao retorno = new RetornoAutenticacao();
                retorno.usuario = resultado;
                retorno.Token = GerarTokenJwt();
                return retorno;
            }
            else return null;
        }

        /// <summary>
        /// Verifica se o usuário existe e se a senha está certa
        /// </summary>
        /// <param name="login">Usuário e senha</param>
        /// <returns>true ou false</returns>
        private Usuario ValidarUsuario(Login login)
        {
            try
            {
                Usuario usuario = _usuarioHandler.PegarPorEmail(email: login.Email);
                if (usuario != null)
                {
                    if (usuario.Senha == login.Senha) return usuario;
                    else return null;
                }
                else return null;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao validar!" +e);
            }
        }

        /// <summary>
        /// Gera chave JWT para o usuário logado
        /// </summary>
        /// <returns>String da chave</returns>
        public string GerarTokenJwt()
        {
            var emissor          = _configuracao["AutenticacaoJwt:Emissor"];
            var destinatarios    = _configuracao["AutenticacaoJwt:Destinatarios"];
            var expiraEm         = DateTime.Now.AddMinutes(600);
            var chaveDeSeguranca = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracao["AutenticacaoJwt:Chave"]));
            var credenciais      = new SigningCredentials(chaveDeSeguranca, SecurityAlgorithms.HmacSha256);
            var token            = new JwtSecurityToken(
                                    issuer:             emissor,
                                    audience:           destinatarios,
                                    expires:            expiraEm,
                                    signingCredentials: credenciais);
            var tokenHandler     = new JwtSecurityTokenHandler();
            var stringToken      = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}
