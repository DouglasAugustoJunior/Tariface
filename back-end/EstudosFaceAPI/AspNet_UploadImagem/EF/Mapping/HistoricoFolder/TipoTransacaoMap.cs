using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping.HistoricoFolder
{
    public class TipoTransacaoMap: IEntityTypeConfiguration<TipoTransacao>
    {
        public void Configure(EntityTypeBuilder<TipoTransacao> builder)
        {
            builder.ToTable(name: "TipoTransacao", schema: "dbo");

            builder.HasMany(e => e.Historicos).WithOne(e => e.TipoTransacao).HasForeignKey(e => e.TipoID);
        }
    }
}
