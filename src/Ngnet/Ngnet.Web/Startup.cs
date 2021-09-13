using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ngnet.Data;
using Ngnet.Web.Infrastructure;

namespace Ngnet.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAutoMapper()
                .AddDatabase(this.Configuration)
                .AddDbContext<NgnetDbContext>()
                .AddIdentity()
                .AddAuthorization()
                .AddJwtAuthentication(services.GetApplicationSettings(this.Configuration))
                .AddServices(this.Configuration)
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseCors(options => options
                      .AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod())
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
                .ApplyMigrations(this.Configuration);
        }
    }
}
