using System.Linq;
using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.Imagens
{
    public class EFImagemRepository : EFRepository<Imagem>, IImagemRepository
    {
        public EFImagemRepository(ContextoDBAplicacao contexto) : base(contexto) { }

        public Imagem PegarImagemPerfil(int idUsuario)
        {
            return Contexto.Imagem
                .Where(i => i.Perfil == true && i.IDUsuario == idUsuario)
                .FirstOrDefault();
        }

        public List<Imagem> PegarPorIDUsuario(int idUsuario)
        {
            return Contexto.Imagem
                .Where(i => i.IDUsuario == idUsuario)
                .ToList();
        }
    }
}
