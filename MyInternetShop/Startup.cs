using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyInternetShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyInternetShop.Data.EntityFactories;

namespace MyInternetShop
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InternetShopContext>();
            services.AddScoped<IInternetShopRepository, InternetShopRepository>();
            services.AddScoped<IClientFactory, ClientFactory>();
            services.AddScoped<IOrderFactory, OrderFactory>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMvc()
                 .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                 .AddNewtonsoftJson();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllers();
            });
        }
    }
}
    

