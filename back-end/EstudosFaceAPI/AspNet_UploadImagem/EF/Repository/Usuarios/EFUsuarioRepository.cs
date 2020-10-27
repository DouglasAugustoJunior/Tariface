using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspNet_UploadImagem.EF.Repository.Usuarios
{
    public class EFUsuarioRepository: EFRepository<Usuario>, IUsuarioRepository
    {
        public EFUsuarioRepository(ContextoDBAplicacao contexto): base(contexto) {  }

        public Usuario PegaComIncludes(int ID)
        {
            return Contexto.Usuario
                .Include(x => x.GrupoPessoa)
                .Include(x => x.Endereco)
                    .ThenInclude(y => y.Municipio)
                        .ThenInclude(z => z.UF)
                            .ThenInclude(w => w.Regiao)
                .Include(x => x.Cartoes)
                .Include(x => x.Historicos)
                    .ThenInclude(h => h.TipoTransacao)
                .Include(x => x.Historicos)
                    .ThenInclude(h => h.StatusTransacao)
                .Include(x => x.Imagens)
                .Where(x => x.ID == ID)
                .FirstOrDefault();
        }

        public Usuario PegaPorPersonID(Guid personID)
        {
            return Contexto.Usuario
                .Where(u => u.PersonId == personID)
                .FirstOrDefault();
        }

        public Usuario PegarPorEmail(string email)
        {
            return Contexto.Usuario
                .Where(u => u.Email.Equals(email))
                .FirstOrDefault();
        }
    }
}
