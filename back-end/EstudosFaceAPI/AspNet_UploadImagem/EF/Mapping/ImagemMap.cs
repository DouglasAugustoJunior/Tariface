using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping
{
    public class ImagemMap : IEntityTypeConfiguration<Imagem>
    {
        public void Configure(EntityTypeBuilder<Imagem> builder)
        {
            builder.ToTable(name: "Imagem", schema: "dbo");

            builder.HasOne(e => e.Usuario).WithMany(e => e.Imagens).HasForeignKey(e => e.IDUsuario); // Um usuário para várias imagens
        }
    }
}
