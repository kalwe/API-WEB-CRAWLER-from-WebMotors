using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using VeiculosWebApi.DbContext;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;
using VeiculosWebApi.Repositories;
using VeiculosWebApi.Services;

namespace VeiculosWebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Add framework services.
            services.AddMvc();

            // IoC
            // Database Context
            services.AddTransient<IVeiculosDbContext, VeiculosDbContext>();

            // Repositories
            // services.AddSingleton(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddSingleton<ICategoriaRepository, CategoriaRepository>();
            services.AddSingleton<IMarcaRepository, MarcaRepository>();
            services.AddSingleton<IModeloRepository, ModeloRepository>();

            // Services
            services.AddTransient(typeof(ISwitchActiveStatusService<>), typeof(SwitchActiveStatusService<>));
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IMarcaService, MarcaService>();
            services.AddTransient<IModeloService, ModeloService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
