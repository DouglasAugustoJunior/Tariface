using System;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using AspNet_UploadImagem.EF.Repository.HistoricoFolder;
using AspNet_UploadImagem.Models.HistoricoFolder.Enums;

namespace AspNet_UploadImagem.Handlers.HistoricoFolder
{
    public class HistoricoHandler
    {
        #region Constantes
        private readonly IHistoricoRepository _historicoRepository;
        #endregion Constantes

        public HistoricoHandler(IHistoricoRepository historicoRepository)
        {
            _historicoRepository = historicoRepository;
        }

        /// <summary>
        /// Recupera lista por ID do usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <returns>Lista de histórico</returns>
        internal IList<Historico> PegaPorUsuarioID(int usuarioId)
        {
            try
            {
                return _historicoRepository.PegaPorIDUsuario(usuarioId: usuarioId);
            }catch(Exception e)
            {
                throw new Exception("Erro ao resgatar por ID do usuário.", e);
            }
        }

        /// <summary>
        /// Cria o histórico da transação
        /// </summary>
        /// <param name="usuario">Usuário da transacao</param>
        /// <param name="tipoTransacao">Tipo da transacao</param>
        /// <param name="statusTransacao">Status da transacao</param>
        /// <param name="valor">Valor da transacao</param>
        /// <returns>True para sucesso e false para erro</returns>
        public bool CriaHistorico(Usuario usuario, ETipoTransacao tipoTransacao, EStatusTransacao statusTransacao, decimal valor)
        {
            if (usuario.ID > 0)
            {
                Historico historico = new Historico
                {
                    IDUsuario = usuario.ID,
                    StatusID  = (int)statusTransacao,
                    TipoID    = (int)tipoTransacao,
                    Valor     = valor
                };
                if (_historicoRepository.Salvar(objeto: historico) != null) return true;
                else return false;
            }
            else return false;
        }
    }
}
