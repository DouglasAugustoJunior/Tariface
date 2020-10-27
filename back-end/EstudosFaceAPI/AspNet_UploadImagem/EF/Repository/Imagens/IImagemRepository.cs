using AspNet_UploadImagem.Models;
using System.Collections.Generic;

namespace AspNet_UploadImagem.EF.Repository.Imagens
{
    public interface IImagemRepository : IRepository<Imagem>
    {
        List<Imagem> PegarPorIDUsuario(int IDUsuario);
        Imagem PegarImagemPerfil(int idUsuario);
    }
}
