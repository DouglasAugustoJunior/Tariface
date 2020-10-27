using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping.EnderecoFolder
{
    public class MunicipioMap: IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder.ToTable(name: "Municipio", schema: "dbo");

            builder.HasOne(e => e.UF).WithMany(e => e.Municipios).HasForeignKey(e => e.UfID); // Um UF para muitos municipios

            builder.HasMany(e => e.Enderecos).WithOne(e => e.Municipio).HasForeignKey(e => e.MunicipioID); // Muitos endereços para um municipio
        }
    }
}
