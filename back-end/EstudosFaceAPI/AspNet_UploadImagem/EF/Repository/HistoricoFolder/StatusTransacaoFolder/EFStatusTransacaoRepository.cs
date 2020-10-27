using AspNet_UploadImagem.Models;

namespace AspNet_UploadImagem.EF.Repository.HistoricoFolder.StatusTransacaoFolder
{
    public class EFStatusTransacaoRepository: EFRepository<StatusTransacao>, IStatusTransacaoRepository
    {
        public EFStatusTransacaoRepository(ContextoDBAplicacao contexto): base(contexto: contexto) { }
    }
}
