using AspNet_UploadImagem.Models;
using Microsoft.EntityFrameworkCore;
using AspNet_UploadImagem.EF.Mapping;
using AspNet_UploadImagem.EF.Mapping.EnderecoFolder;
using AspNet_UploadImagem.EF.Mapping.HistoricoFolder;

namespace AspNet_UploadImagem.EF
{
    public class ContextoDBAplicacao: DbContext
    {

        public ContextoDBAplicacao(DbContextOptions<ContextoDBAplicacao> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new GrupoPessoaMap());
            modelBuilder.ApplyConfiguration(new ImagemMap());
            modelBuilder.ApplyConfiguration(new CartaoMap());
            modelBuilder.ApplyConfiguration(new EnderecoMap());
            modelBuilder.ApplyConfiguration(new RegiaoMap());
            modelBuilder.ApplyConfiguration(new UFMap());
            modelBuilder.ApplyConfiguration(new MunicipioMap());
            modelBuilder.ApplyConfiguration(new HistoricoMap());
            modelBuilder.ApplyConfiguration(new TipoTransacaoMap());
            modelBuilder.ApplyConfiguration(new StatusTransacaoMap());

            base.OnModelCreating(modelBuilder);
        }

        #region DbSets
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<GrupoPessoa> GrupoPessoa { get; set; }
        public virtual DbSet<Imagem> Imagem { get; set; }
        public virtual DbSet<Cartao> Cartao { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Regiao> Regiao { get; set; }
        public virtual DbSet<UF> UF { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<Historico> Historico { get; set; }
        public virtual DbSet<TipoTransacao> TipoTransacao { get; set; }
        public virtual DbSet<StatusTransacao> StatusTransacao { get; set; }
        #endregion DbSets
    }
}
