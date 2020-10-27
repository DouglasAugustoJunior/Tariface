using System;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using AspNet_UploadImagem.EF.Repository.HistoricoFolder.StatusTransacaoFolder;

namespace AspNet_UploadImagem.Handlers.HistoricoFolder
{
    public class StatusTransacaoHandler
    {
        #region Constantes
        private readonly IStatusTransacaoRepository _statusTransacaoRepository;
        #endregion Constantes

        public StatusTransacaoHandler(IStatusTransacaoRepository statusTransacaoRepository)
        {
            _statusTransacaoRepository = statusTransacaoRepository;
        }

        /// <summary>
        /// Retorna uma lista de status
        /// </summary>
        /// <returns>Lista de status</returns>
        internal IList<StatusTransacao> PegaTodosStatus()
        {
            try
            {
                return _statusTransacaoRepository.PegarTudo();
            }catch(Exception e)
            {
                throw new Exception("Erro ao recuperar lista de status.", e);
            }
        }
    }
}
