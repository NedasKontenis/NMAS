using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NMAS.WebApi.Repositories;
using NMAS.WebApi.Repositories.AccommodationPlaceEntity;
using NMAS.WebApi.Repositories.IllegalMigrantEntity;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using NMAS.WebApi.Services.IllegalMigrantEntity;

namespace NMAS.WebApi.Host.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddSingleton(new RepositoryConfiguration
            {
                DefaultConnectionString = configuration.GetConnectionString("DefaultConnection")
            });

            services.AddTransient<IIllegalMigrantEntityRepository, IllegalMigrantEntityRepository>();
            services.AddTransient<IIllegalMigrantEntityService, IllegalMigrantEntityService>();

            services.AddTransient<IAccommodationPlaceEntityRepository, AccommodationPlaceEntityRepository>();
            services.AddTransient<IAccommodationPlaceEntityService, AccommodationPlaceEntityService>();
        }
    }
}
