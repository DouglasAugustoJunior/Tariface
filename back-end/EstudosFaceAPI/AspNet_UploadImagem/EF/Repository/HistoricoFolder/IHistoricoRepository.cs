using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.HistoricoFolder
{
    public interface IHistoricoRepository: IRepository<Historico>
    {
        /// <summary>
        /// Retorna os históricos por CPF do usuário
        /// </summary>
        /// <param name="usuarioId">CPF do usuário</param>
        /// <returns>Lista de históricos do usuário</returns>
        IList<Historico> PegaPorIDUsuario(int usuarioId);
    }
}
