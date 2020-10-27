using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping.EnderecoFolder
{
    public class RegiaoMap: IEntityTypeConfiguration<Regiao>
    {
        public void Configure(EntityTypeBuilder<Regiao> builder)
        {
            builder.ToTable(name: "Regiao", schema: "dbo");

                builder.HasMany(e => e.UFs).WithOne(e => e.Regiao).HasForeignKey(e => e.RegiaoID);
        }
    }
}
