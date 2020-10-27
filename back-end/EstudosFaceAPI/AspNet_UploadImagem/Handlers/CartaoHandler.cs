using System;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using AspNet_UploadImagem.EF.Repository.CartaoFolder;

namespace AspNet_UploadImagem.Handlers
{
    public class CartaoHandler
    {
        #region Constantes
        private readonly ICartaoRepository _cartaoRepository;
        #endregion Constantes

        public CartaoHandler(ICartaoRepository cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
        }

        /// <summary>
        /// Retorna uma lista de cartões com base no ID do usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <returns>Lista de cartões</returns>
        internal IList<Cartao> PegaPorIDUsuario(int usuarioId)
        {
            try
            {
                return _cartaoRepository.PegaPorIDUsuario(usuarioId: usuarioId);
            }catch(Exception e)
            {
                throw new Exception("Ocorreu um erro ao buscar.", e);
            }
        }

        /// <summary>
        /// Cria um novo registro de cartão
        /// </summary>
        /// <param name="cartao">Objeto recebido pelo front</param>
        /// <returns>String de sucesso ou falha</returns>
        internal string CriaCartao(Cartao cartao)
        {
            try
            {
                var result = _cartaoRepository.Salvar(objeto: cartao);
                if (result != null) return "Cadastrado com sucesso!";
                else return "Ocorreu um erro ao salvar, por favor, tente novamente.";
            }catch(Exception e)
            {
                throw new Exception("Ocorreu um erro ao salvar", e);
            }

        }

        /// <summary>
        /// Atualiza um cartão
        /// </summary>
        /// <param name="cartao">Objeto a ser atualizado</param>
        /// <returns>String de sucesso ou falha</returns>
        internal string AtualizaCartao(Cartao cartao)
        {
            try
            {
                var result = _cartaoRepository.Atualizar(objeto: cartao);
                if (result != null) return "Atualizado com sucesso!";
                else return "Ocorreu um erro ao atualizar.";
            }catch(Exception e)
            {
                throw new Exception("Ocorreu um erro ao tentar atualizar.", e);
            }
        }

        /// <summary>
        /// Deleta um cartão
        /// </summary>
        /// <param name="id">ID do objeto a ser excluído</param>
        /// <returns>String com sucesso ou falha</returns>
        internal string ExcluiCartao(int id)
        {
            try
            {
                Cartao cartao = new Cartao();
                cartao.ID = id;
                if (_cartaoRepository.Apagar(objeto: cartao)) return "Excluído com sucesso!";
                else return "Erro ao excluir.";
            }catch(Exception e)
            {
                throw new Exception("Erro ao excluír.", e);
            }
        }
    }
}
