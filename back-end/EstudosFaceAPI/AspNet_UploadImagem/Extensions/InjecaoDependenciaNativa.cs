using AspNet_UploadImagem.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNet_UploadImagem.EF.Repository.Grupos;
using AspNet_UploadImagem.EF.Repository.Imagens;
using AspNet_UploadImagem.EF.Repository.Usuarios;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder;
using AspNet_UploadImagem.EF.Repository.HistoricoFolder;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder.RegiaoFolder;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder.UFFolder;
using AspNet_UploadImagem.EF.Repository.EnderecoFolder.MunicipioFolder;
using AspNet_UploadImagem.EF.Repository.HistoricoFolder.TipoTransacaoFolder;
using AspNet_UploadImagem.EF.Repository.HistoricoFolder.StatusTransacaoFolder;
using AspNet_UploadImagem.EF.Repository.CartaoFolder;
using AspNet_UploadImagem.Handlers.HistoricoFolder;
using AspNet_UploadImagem.Handlers.EnderecoFolder;

namespace AspNet_UploadImagem.Extensions
{
    public class InjecaoDependenciaNativa
    {
        /// <summary>
        /// Registra todos os serviços necessários
        /// </summary>
        /// <param name="services">Coleção de serviços que guarda os serviços da aplicação</param>
        public static void RegistraServicos(IServiceCollection services)
        {
            CarregaHandlers(services: services);
            CarregaRepositorys(services: services);
        }

        /// <summary>
        /// Carrega as Handlers para ser usado em instância
        /// </summary>
        /// <param name="services">Serviço para inserir as handlers</param>
        private static void CarregaHandlers(IServiceCollection services)
        {
            services.AddScoped<UsuarioHandler, UsuarioHandler>();
            services.AddScoped<LoginHandler,   LoginHandler>();
            services.AddScoped<ImagemHandler,  ImagemHandler>();
            services.AddScoped<CartaoHandler,  CartaoHandler>();


            services.AddScoped<EnderecoHandler,  EnderecoHandler>();
            services.AddScoped<MunicipioHandler, MunicipioHandler>();
            services.AddScoped<UFHandler,        UFHandler>();
            services.AddScoped<RegiaoHandler,    RegiaoHandler>();


            services.AddScoped<HistoricoHandler,       HistoricoHandler>();
            services.AddScoped<StatusTransacaoHandler, StatusTransacaoHandler>();
            services.AddScoped<TipoTransacaoHandler,   TipoTransacaoHandler>();
        }

        /// <summary>
        /// Carrega as repositorys que vão tratar a comunicação com o banco de dados
        /// </summary>
        /// <param name="services">Serviço para inserir as repositorys</param>
        private static void CarregaRepositorys(IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository,         EFUsuarioRepository>();
            services.AddScoped<IGrupoPessoaRepository,     EFGrupoPessoaRepository>();
            services.AddScoped<IImagemRepository,          EFImagemRepository>();
            services.AddScoped<ICartaoRepository,          EFCartaoRepository>();


            services.AddScoped<IEnderecoRepository,        EFEnderecoRepository>();
            services.AddScoped<IRegiaoRepository,          EFRegiaoRepository>();
            services.AddScoped<IUFRepository,              EFUFRepository>();
            services.AddScoped<IMunicipioRepository,       EFMunicipioRepository>();


            services.AddScoped<IHistoricoRepository,       EFHistoricoRepository>();
            services.AddScoped<ITipoTransacaoRepository,   EFTipoTransacaoRepository>();
            services.AddScoped<IStatusTransacaoRepository, EFStatusTransacaoRepository>();
        }
    }
}
