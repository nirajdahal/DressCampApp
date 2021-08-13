using API.Extensions;
using API.Middlewares;
using Core.Interfaces;
using Core.Models;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddControllers();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureIdentity(Configuration);
            
            services.AddScoped<JwtHandler>();
            services.ConfigureCors();
            services.ConfigureMailService(Configuration);
            services.ConfigureRepositories();
            services.AddScoped<IBackgroundService, Infrastructure.Services.BackgroundService>();
            services.AddAutoMapper(typeof(Startup));
            //order for validation error matters
            services.ConfigureValidationError();
            services.ConfigureRedis(Configuration);
            services.ConfigureHangfireContext(Configuration); 
            services.AddHangfireServer();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //Middleware for handling 500 error
            app.UseMiddleware<DefaultExceptionHandlerMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireCustomBasicAuthenticationFilter { User = Configuration.GetValue<string> ("Hangfire:UserName"), Pass = Configuration.GetValue<string> ("Hangfire:Password") }
                }
            });

            

        }
    }
}
