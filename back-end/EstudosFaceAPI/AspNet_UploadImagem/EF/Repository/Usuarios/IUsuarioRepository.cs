using AspNet_UploadImagem.Models;
using System;

namespace AspNet_UploadImagem.EF.Repository.Usuarios
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario PegaComIncludes(int ID);
        Usuario PegaPorPersonID(Guid personID);
        Usuario PegarPorEmail(string email);
    }
}
