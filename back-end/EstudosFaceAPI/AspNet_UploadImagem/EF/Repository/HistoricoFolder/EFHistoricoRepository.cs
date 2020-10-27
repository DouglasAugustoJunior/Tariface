using System.Linq;
using System.Collections.Generic;
using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNet_UploadImagem.EF.Repository.HistoricoFolder
{
    public class EFHistoricoRepository: EFRepository<Historico>, IHistoricoRepository
    {
        public EFHistoricoRepository(ContextoDBAplicacao contexto): base(contexto: contexto) { }

        public IList<Historico> PegaPorIDUsuario(int idUsuario)
        {
            return Contexto.Historico
                .Include(t => t.TipoTransacao)
                .Include(s => s.StatusTransacao)
                .Where(h => h.IDUsuario == idUsuario)
                .ToList();
        }
    }
}
