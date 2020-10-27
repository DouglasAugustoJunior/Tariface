using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet_UploadImagem.EF.Mapping
{
    public class UsuarioMap: IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable(name: "Usuario", schema: "dbo");
            builder.Property(e => e.Saldo).HasColumnName("Saldo").HasColumnType("decimal(8,2)");

            builder.HasMany(e => e.Imagens).WithOne(e => e.Usuario).HasForeignKey(e => e.IDUsuario);    // Muitas Imagens para um usuário
            builder.HasMany(e => e.Cartoes).WithOne(e => e.Usuario).HasForeignKey(e => e.IDUsuario);    // Muitos Cartões para um usuário
            builder.HasMany(e => e.Historicos).WithOne(e => e.Usuario).HasForeignKey(e => e.IDUsuario); // Muitos Históricos para um usuário

            builder.HasOne(e => e.GrupoPessoa).WithMany(e => e.Usuarios).HasForeignKey(e => e.GrupoPessoaID); // Um grupo para muitos usuários
            builder.HasOne(e => e.Endereco).WithMany().HasForeignKey(e => e.EnderecoID);                      // Um endereço para muitos usuários(não tem chave do usuário no endereço)
        }
    }
}
