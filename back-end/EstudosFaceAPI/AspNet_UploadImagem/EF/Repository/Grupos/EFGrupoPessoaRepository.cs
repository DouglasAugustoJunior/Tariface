using AspNet_UploadImagem.Models;
using System.Linq;

namespace AspNet_UploadImagem.EF.Repository.Grupos
{
    public class EFGrupoPessoaRepository : EFRepository<GrupoPessoa>, IGrupoPessoaRepository
    {
        public EFGrupoPessoaRepository(ContextoDBAplicacao contexto) : base(contexto)
        {

        }

        public GrupoPessoa PegarPrimeiro()
        {
            return Contexto.GrupoPessoa.Where(x => x.ID != 0).FirstOrDefault();
        }
    }
}
