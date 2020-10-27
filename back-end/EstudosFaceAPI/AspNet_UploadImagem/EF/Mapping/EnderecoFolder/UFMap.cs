using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping.EnderecoFolder
{
    public class UFMap: IEntityTypeConfiguration<UF>
    {
        public void Configure(EntityTypeBuilder<UF> builder)
        {
            builder.ToTable(name: "UF", schema: "dbo");
            builder.HasOne(e => e.Regiao).WithMany(e => e.UFs).HasForeignKey(e => e.RegiaoID); // Uma região para muitos UFs

            builder.HasMany(e => e.Municipios).WithOne(e => e.UF).HasForeignKey(e => e.UfID); // Muitos municipios para um UF
        }
    }
}
