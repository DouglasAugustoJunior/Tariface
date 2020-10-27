using System;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder.RegiaoFolder;

namespace AspNet_UploadImagem.Handlers.EnderecoFolder
{
    public class RegiaoHandler
    {
        #region Constantes
        private readonly IRegiaoRepository _regiaoRepository;
        #endregion Constantes

        public RegiaoHandler(IRegiaoRepository regiaoRepository)
        {
            _regiaoRepository = regiaoRepository;
        }

        /// <summary>
        /// Retorna lista de Regiões
        /// </summary>
        /// <returns>Lista de Regiões</returns>
        internal IList<Regiao> PegaTodasRegioes()
        {
            try
            {
                return _regiaoRepository.PegarTudo();
            }catch(Exception e)
            {
                throw new Exception("Erro ao resgatar lista de Regiões.", e);
            }
        }
    }
}
