using Core.Entities.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
       services.AddCors(options =>
       {
           options.AddPolicy("CorsPolicy", builder =>
               builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
       });


        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
                   services.AddDbContext<RepositoryContext>(opts =>
                       opts.UseSqlServer(configuration.GetConnectionString("SqlConnection"), b => b.MigrationsAssembly("API")));
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<RepositoryContext>();
        }
    }
}
