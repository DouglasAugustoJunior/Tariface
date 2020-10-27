using System;
using AspNet_UploadImagem.Models;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder;

namespace AspNet_UploadImagem.Handlers.EnderecoFolder
{
    public class EnderecoHandler
    {
        #region Constantes
        private readonly IEnderecoRepository _enderecoRepository;
        #endregion Constantes

        public EnderecoHandler(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        /// <summary>
        /// PEgar por ID com todos os includes
        /// </summary>
        /// <param name="ID">ID do endereço</param>
        /// <returns>Objeto Endereco</returns>
        internal Endereco PegaPorID(int ID)
        {
            try
            {
                return _enderecoRepository.PegarPorIDComIncludes(ID: ID);
            }catch(Exception e)
            {
                throw new Exception("Erro ao resgatar por ID.", e);
            }
        }

        /// <summary>
        /// Cria um novo endereço
        /// </summary>
        /// <param name="endereco">Objeto a ser salvo</param>
        /// <returns>String de sucesso ou falha</returns>
        internal string CriaEndereco(Endereco endereco)
        {
            try
            {
                var result = _enderecoRepository.Salvar(objeto: endereco);
                if (result != null) return "Salvo com sucesso!";
                else return "Erro ao salvar.";
            }catch(Exception e)
            {
                throw new Exception("Erro ao criar endereço.", e);
            }
        }

        /// <summary>
        /// Atualiza o endereço
        /// </summary>
        /// <param name="endereco">Objeto endereço</param>
        /// <returns>String de sucesso ou falha</returns>
        internal string AtualizaEndereco(Endereco endereco)
        {
            try
            {
                var result = _enderecoRepository.Atualizar(objeto: endereco);
                if (result != null) return "Atualizado com sucesso!";
                else return "Erro ao atualizar endereço.";
            }catch(Exception e)
            {
                throw new Exception("Erro ao atualizar endereço.", e);
            }
        }

        /// <summary>
        /// Excluí endereço pelo ID
        /// </summary>
        /// <param name="endereco">ID do endereço a ser excluído</param>
        /// <returns>Retorna string de sucesso ou falha</returns>
        internal string ExcluiEndereco(Endereco endereco)
        {
            try
            {
                if (_enderecoRepository.Apagar(objeto: endereco)) return "Excluído com sucesso.";
                else return "Erro ao excluír.";
            }catch(Exception e)
            {
                throw new Exception("Erro ao excluir endereço.", e);
            }
        }
    }
}
