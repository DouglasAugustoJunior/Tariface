using System.Linq;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.CartaoFolder
{
    public class EFCartaoRepository: EFRepository<Cartao>, ICartaoRepository
    {
        public EFCartaoRepository(ContextoDBAplicacao contexto): base(contexto: contexto) { }

        public IList<Cartao> PegaPorIDUsuario(int idUsuario)
        {
            return Contexto.Cartao.Where(c => c.IDUsuario == idUsuario).ToList();
        }
    }
}
