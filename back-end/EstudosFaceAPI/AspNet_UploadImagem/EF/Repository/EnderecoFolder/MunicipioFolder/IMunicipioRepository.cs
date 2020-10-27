using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.EnderecoFolder.MunicipioFolder
{
    public interface IMunicipioRepository : IRepository<Municipio>
    {
        IList<Municipio> PegarPorUFID(int idUF);
    }
}
