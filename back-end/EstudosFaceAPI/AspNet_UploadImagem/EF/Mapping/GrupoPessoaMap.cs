using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping
{
    public class GrupoPessoaMap : IEntityTypeConfiguration<GrupoPessoa>
    {
        public void Configure(EntityTypeBuilder<GrupoPessoa> builder)
        {
            builder.ToTable(name: "GrupoPessoa", schema: "dbo");

            builder.HasMany(e => e.Usuarios).WithOne(e => e.GrupoPessoa).HasForeignKey(e => e.GrupoPessoaID); // Muitos Usuários para um grupo
        }
    }
}
