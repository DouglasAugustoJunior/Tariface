using AspNet_UploadImagem.Models;

namespace AspNet_UploadImagem.EF.Repository.HistoricoFolder.TipoTransacaoFolder
{
    public class EFTipoTransacaoRepository: EFRepository<TipoTransacao>, ITipoTransacaoRepository
    {
        public EFTipoTransacaoRepository(ContextoDBAplicacao contexto): base(contexto: contexto){

        }
    }
}
