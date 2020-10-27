using System;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using AspNet_UploadImagem.EF.Repository.HistoricoFolder.TipoTransacaoFolder;

namespace AspNet_UploadImagem.Handlers.HistoricoFolder
{
    public class TipoTransacaoHandler
    {
        #region Constantes
        private readonly ITipoTransacaoRepository _tipoTransacaoRepository;
        #endregion Constantes

        public TipoTransacaoHandler(ITipoTransacaoRepository tipoTransacaoRepository)
        {
            _tipoTransacaoRepository = tipoTransacaoRepository;
        }

        /// <summary>
        /// Retorna uma lista de tipos
        /// </summary>
        /// <returns>Lista de tipos</returns>
        internal IList<TipoTransacao> PegaTodosTipos()
        {
            try
            {
                return _tipoTransacaoRepository.PegarTudo();
            }catch(Exception e)
            {
                throw new Exception("Erro ao buscar tipos.", e);
            }
        }
    }
}
