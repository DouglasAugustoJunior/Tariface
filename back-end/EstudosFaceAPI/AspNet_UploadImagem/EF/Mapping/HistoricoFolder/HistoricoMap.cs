using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping.HistoricoFolder
{
    public class HistoricoMap: IEntityTypeConfiguration<Historico>
    {
        public void Configure(EntityTypeBuilder<Historico> builder)
        {
            builder.ToTable(name: "Historico", schema: "dbo");
            builder.Property(e => e.Valor).HasColumnName("Valor").HasColumnType("decimal(8,2)");

            builder.HasOne(e => e.Usuario).WithMany(e => e.Historicos).HasForeignKey(e => e.IDUsuario); // Um usuário para muitos históricos
            builder.HasOne(e => e.StatusTransacao).WithMany(e => e.Historicos).HasForeignKey(e => e.StatusID); // Um Status para muitos históricos
            builder.HasOne(e => e.TipoTransacao).WithMany(e => e.Historicos).HasForeignKey(e => e.TipoID); // Um Tipo para muitos históricos

        }
    }
}
