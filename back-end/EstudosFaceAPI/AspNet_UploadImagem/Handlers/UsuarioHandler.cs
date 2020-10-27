using System;
using AspNet_UploadImagem.Models;
using AspNet_UploadImagem.EF.Repository.Grupos;
using AspNet_UploadImagem.EF.Repository.Usuarios;
using AspNet_UploadImagem.Handlers.HistoricoFolder;
using AspNet_UploadImagem.Models.HistoricoFolder.Enums;

namespace AspNet_UploadImagem.Handlers
{
    public class UsuarioHandler
    {
        #region Constantes
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IGrupoPessoaRepository _grupoPessoaRepository;
        private readonly HistoricoHandler _historicoHandler;
        #endregion Constantes

        public UsuarioHandler(
            IUsuarioRepository usuarioRepository,
            IGrupoPessoaRepository grupoPessoaRepository,
            HistoricoHandler historicoHandler)
        {
            _usuarioRepository     = usuarioRepository;
            _grupoPessoaRepository = grupoPessoaRepository;
            _historicoHandler      = historicoHandler;
        }

        /// <summary>
        /// Pega o usuário pelo e-mail
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Usuário</returns>
        internal Usuario PegarPorEmail(string email)
        {
            try
            {
                return _usuarioRepository.PegarPorEmail(email: email);
            }catch(Exception e)
            {
                throw new Exception("Erro ao buscar usuário por Email." + e);
            }
        }

        /// <summary>
        /// Pega o usuário pelo ID com todos os includes
        /// </summary>
        /// <param name="ID">ID do usuário</param>
        /// <returns>Objeto do usuário com todos os Includes</returns>
        internal Usuario PegaPorID(int ID)
        {
            try
            {
                if (ID > 0)
                {
                    return _usuarioRepository.PegaComIncludes(ID: ID);
                }
                else throw new Exception("ID do usuário inválido!");
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao buscar: " + e);
            }
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="usuario">Usuário</param>
        /// <returns>ID do usuário criado</returns>
        public int CriaUsuario(Usuario usuario)
        {
            try
            {
                GrupoPessoa grupo = _grupoPessoaRepository.PegarPrimeiro();
                usuario.GrupoPessoaID = grupo.ID;
                usuario.Saldo = 0; // Ao criar o saldo sempre será zerado.
                Usuario resultados = _usuarioRepository.PegarPorEmail(email: usuario.Email);
                if (resultados != null) throw new Exception("Já existe um usuário com esse email.");
                Usuario usuarioRetorno = _usuarioRepository.Salvar(usuario);
                return usuarioRetorno.ID;
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao criar: " + e);
            }
        }

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        /// <param name="usuario">Usuário a ser atualizado</param>
        /// <returns>Usuário atualizado</returns>
        internal Usuario AtualizarUsuario(Usuario usuario)
        {
            try
            {
                Usuario usuarioRetorno = _usuarioRepository.Atualizar(objeto: usuario);
                if (usuarioRetorno != null) return usuarioRetorno;
                else throw new Exception("Erro ao atualizar usuário.");
            }catch(Exception e)
            {
                throw new Exception("Erro ao atualizar usuário.", e);
            }
        }

        /// <summary>
        /// Adiciona crédito ao usuário
        /// </summary>
        /// <param name="idUsuario">ID do usuário</param>
        /// <param name="valor">Valor a ser adicionado</param>
        /// <returns></returns>
        internal string AdicionaSaldo(int idUsuario, decimal valor)
        {
            try
            {
                if(idUsuario > 0 && valor > 0)
                {
                    Usuario usuario = _usuarioRepository.PegarPorID(ID: idUsuario);
                    usuario.Saldo += valor;
                    var retorno = _usuarioRepository.Atualizar(objeto: usuario);

                    if (retorno != null)
                    {
                        _historicoHandler.CriaHistorico(usuario: usuario, ETipoTransacao.Credito, EStatusTransacao.Concluida, valor);
                        return $"Novo saldo: R$ {usuario.Saldo}";
                    }
                    else {
                        _historicoHandler.CriaHistorico(usuario: usuario, ETipoTransacao.Credito, EStatusTransacao.Cancelada, valor);
                        return $"Operação cancelada, saldo atual R$ {usuario.Saldo - valor}";
                    }
                }
                else
                {
                    throw new Exception("ID do usuário ou valor inválidos!");
                }
            }catch(Exception e)
            {
                throw new Exception("Ocorreu um erro ao adicionar o saldo.", e);
            }
        }
    }
}
