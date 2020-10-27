using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping
{
    public class CartaoMap : IEntityTypeConfiguration<Cartao>
    {
        public void Configure(EntityTypeBuilder<Cartao> builder)
        {
            builder.ToTable(name: "Cartao", schema: "dbo");

            builder.HasOne(e => e.Usuario).WithMany(e => e.Cartoes).HasForeignKey(e => e.IDUsuario); // Um usuário para vários cartões

        }
    }
}
