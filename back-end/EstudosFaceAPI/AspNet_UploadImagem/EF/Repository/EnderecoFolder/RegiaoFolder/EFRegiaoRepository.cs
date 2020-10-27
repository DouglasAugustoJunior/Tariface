using AspNet_UploadImagem.Models;

namespace AspNet_UploadImagem.EF.Repository.EnderecoFolder.RegiaoFolder
{
    public class EFRegiaoRepository: EFRepository<Regiao>, IRegiaoRepository
    {
        public EFRegiaoRepository(ContextoDBAplicacao contexto): base(contexto: contexto) { }
    }
}
