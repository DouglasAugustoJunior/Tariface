using System;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder.UFFolder;

namespace AspNet_UploadImagem.Handlers.EnderecoFolder
{
    public class UFHandler
    {
        #region Constantes
        private readonly IUFRepository _uFRepository;
        #endregion Constantes

        public UFHandler(IUFRepository uFRepository)
        {
            _uFRepository = uFRepository;
        }

        /// <summary>
        /// Retorna lista de UFs
        /// </summary>
        /// <returns>Lista de UFs</returns>
        internal IList<UF> PegaTodosUFs()
        {
            try
            {
                return _uFRepository.PegarTudo();
            }catch(Exception e)
            {
                throw new Exception("Erro ao resgatar UFs.",e);
            }
        }
    }
}
