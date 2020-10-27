using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping.EnderecoFolder
{
    public class EnderecoMap : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable(name: "Endereco", schema: "dbo");

            builder.HasOne(e => e.Municipio).WithMany(e => e.Enderecos).HasForeignKey(e => e.MunicipioID); // Um Municipio para muitos Endereços

            builder.HasMany(e => e.Usuarios).WithOne(e => e.Endereco).HasForeignKey(e => e.EnderecoID); // Muitos usuários para um endereço
        }
    }
}
