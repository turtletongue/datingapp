using API.Data;
using API.interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
          services.AddDbContext<DataContext>(options =>
          {
            options.UseMySQL(config.GetConnectionString("DefaultConnection"));
          });
          services.AddScoped<ITokenService, TokenService>();

          return services;
        }
    }
}