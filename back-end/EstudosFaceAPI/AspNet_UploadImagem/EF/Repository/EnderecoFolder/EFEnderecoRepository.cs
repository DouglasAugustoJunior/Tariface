using System.Linq;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AspNet_UploadImagem.EF.Repository.EnderecoFolder
{
    public class EFEnderecoRepository : EFRepository<Endereco>, IEnderecoRepository
    {
        public EFEnderecoRepository(ContextoDBAplicacao contexto) : base(contexto)
        {

        }

        public Endereco PegarPorIDComIncludes(int ID)
        {
            return Contexto.Endereco
                .Include(a => a.Municipio)
                    .ThenInclude(b => b.UF)
                        .ThenInclude(c => c.Regiao)
                .Where(d => d.ID == ID)
                .FirstOrDefault();
        }
    }
}
