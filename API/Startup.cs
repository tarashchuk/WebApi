using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using API.Models;
using Microsoft.EntityFrameworkCore;
using API.Controllers;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // получаем строку подключения из файла конфигурации
            string connection = Configuration.GetConnectionString("DBConnection");
            // добавляем контекст DocumentContext в качестве сервиса в приложение
            services.AddDbContext<DocumentContext>(options =>
                options.UseSqlServer(connection));

           // Метод AddScoped создает один экземпляр объекта для всего запроса, таким способом реализует DI (ТОЛЬКО ДЛЯ EF ЧАСТИ)
            services.AddScoped<IDocumentsRepository, EFDocumentsRepository>();
        //    services.AddScoped<IDocumentsRepository, ADODocumentsRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
