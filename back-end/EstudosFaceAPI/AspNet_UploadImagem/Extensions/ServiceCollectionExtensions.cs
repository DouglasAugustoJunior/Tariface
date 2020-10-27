using Microsoft.Extensions.DependencyInjection;

namespace AspNet_UploadImagem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adiciona as injeções de dependência na aplicação
        /// </summary>
        /// <param name="services">Coleção de serviços da aplicação</param>
        /// <param name="configuration">Configuração da aplicação</param>
        public static void AdicionarInjecaoDependencias(this IServiceCollection services)
        {
            InjecaoDependenciaNativa.RegistraServicos(services: services);
        }
    }
}
