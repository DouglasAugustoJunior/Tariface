using AspNet_UploadImagem.Models;

namespace AspNet_UploadImagem.EF.Repository.Grupos
{
    public interface IGrupoPessoaRepository : IRepository<GrupoPessoa>
    {
        GrupoPessoa PegarPrimeiro();
    }
}
