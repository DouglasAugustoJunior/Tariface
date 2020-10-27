using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.EnderecoFolder
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Endereco PegarPorIDComIncludes(int ID);
    }
}
