using AspNet_UploadImagem.Models;

namespace AspNet_UploadImagem.EF.Repository.EnderecoFolder.UFFolder
{
    public class EFUFRepository: EFRepository<UF>, IUFRepository
    {
        public EFUFRepository(ContextoDBAplicacao contexto): base(contexto: contexto) { }
    }
}
