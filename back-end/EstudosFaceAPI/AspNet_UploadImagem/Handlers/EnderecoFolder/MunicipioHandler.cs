using System;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder.MunicipioFolder;

namespace AspNet_UploadImagem.Handlers.EnderecoFolder
{
    public class MunicipioHandler
    {
        #region Constantes
        private readonly IMunicipioRepository _municipioRepository;
        #endregion Constantes

        public MunicipioHandler(IMunicipioRepository municipioRepository)
        {
            _municipioRepository = municipioRepository;
        }

        /// <summary>
        /// Retorna lista de Municípios
        /// </summary>
        /// <returns>Lista de municípios</returns>
        internal IList<Municipio> PegaTodosMunicipios()
        {
            try
            {
                return _municipioRepository.PegarTudo();
            }catch(Exception e)
            {
                throw new Exception("Erro ao recuperar Municípios.", e);
            }
        }

        /// <summary>
        /// Pega municípios por estado
        /// </summary>
        /// <param name="idUF">ID do estado</param>
        /// <returns>Lista de municipios</returns>
        internal IList<Municipio> PegaPorIDUF(int idUF)
        {
            try{
                return _municipioRepository.PegarPorUFID(idUF: idUF);
            }catch(Exception e)
            {
                throw new Exception("Erro ao buscar município." + e);
            }
        }
    }
}
