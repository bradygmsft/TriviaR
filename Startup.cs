using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TriviaR.Hubs;
using TriviaR.Services;

namespace TriviaR
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
            services.AddMvc();
            services.AddCors(options => options.AddPolicy("CorsPolicy", 
            builder => 
            {
                builder
                    .AllowAnyMethod()
                        .AllowAnyHeader()
                            .WithOrigins("http://localhost:55830");
            }));
            services
                .AddSignalR()
                .AddAzureSignalR();

            services.AddTransient<IQuestionDataSource, JsonFileQuestionSource>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<GameHub>("/gamehub");
            });
            app.UseMvc();
        }
    }
}
