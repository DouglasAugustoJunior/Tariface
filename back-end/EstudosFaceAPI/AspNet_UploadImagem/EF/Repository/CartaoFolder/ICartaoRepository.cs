using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.CartaoFolder
{
    public interface ICartaoRepository : IRepository<Cartao>
    {
        /// <summary>
        /// Retorna os cartões por ID do usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <returns>Lista de cartões do usuário</returns>
        IList<Cartao> PegaPorIDUsuario(int usuarioId);
    }
}
