using System.Linq;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.EnderecoFolder.MunicipioFolder
{
    public class EFMunicipioRepository: EFRepository<Municipio>, IMunicipioRepository
    {
        public EFMunicipioRepository(ContextoDBAplicacao contexto): base(contexto: contexto) { }

        public IList<Municipio> PegarPorUFID(int idUF)
        {
            return Contexto.Municipio
                .Where(m => m.UfID == idUF)
                .ToList();
        }
    }
}
