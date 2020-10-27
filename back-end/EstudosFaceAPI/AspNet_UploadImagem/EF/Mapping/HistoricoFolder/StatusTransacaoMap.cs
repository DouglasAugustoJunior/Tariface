using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping.HistoricoFolder
{
    public class StatusTransacaoMap: IEntityTypeConfiguration<StatusTransacao>
    {
        public void Configure(EntityTypeBuilder<StatusTransacao> builder)
        {
            builder.ToTable(name: "StatusTransacao", schema: "dbo");

            builder.HasMany(e => e.Historicos).WithOne(e => e.StatusTransacao).HasForeignKey(e => e.StatusID);

        }
    }
}
