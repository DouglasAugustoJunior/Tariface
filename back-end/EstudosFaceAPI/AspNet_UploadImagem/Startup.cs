using System;
using System.Text;
using AspNet_UploadImagem.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AspNet_UploadImagem.Extensions;
using Microsoft.IdentityModel.Tokens;
using AspNet_UploadImagem.Models.FaceAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;
using AspNet_UploadImagem.Models.AzureStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AspNet_UploadImagem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///  Este método é chamado pelo tempo de execução. Use este método para adicionar serviços ao contêiner.
        /// </summary>
        /// <param name="services">Serviços da aplicação</param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer              = Configuration["AutenticacaoJwt:Emissor"],
                        ValidAudience            = Configuration["AutenticacaoJwt:Destinatarios"],
                        IssuerSigningKey         = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(Configuration["AutenticacaoJwt:Chave"]))
                    };
                }); // Autenticação JWT

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            ); // Evita JSON infinito
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            ); // Evita JSON infinito

            services.Configure<IISOptions>(o => { o.ForwardClientCertificate = false; }); // Configuração para publicar
            services.Configure<FormOptions>(options => {
                options.MemoryBufferThreshold = Int32.MaxValue;
                options.ValueCountLimit = 10; //default 1024
                options.ValueLengthLimit = int.MaxValue; //not recommended value
                options.MultipartBodyLengthLimit = long.MaxValue; //not recommended value
            });
            services.Configure<StorageConfig>(Configuration.GetSection("StorageConfig")); // Pega os valores do appsetting.json
            services.Configure<FaceAPI>(Configuration.GetSection("FaceAPI"));             // Pega os valores do appsetting.json
            services.AddDbContext<ContextoDBAplicacao>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Tariface"))); // Conecta no banco de dados
            services.AddControllers();
            services.AdicionarInjecaoDependencias();
        }

        /// <summary>
        /// Este método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
        /// </summary>
        /// <param name="app">Build da aplicação</param>
        /// <param name="env">Enviroment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            #region JWT
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion JWT
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
