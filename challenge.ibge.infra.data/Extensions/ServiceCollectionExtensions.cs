using challenge.ibge.infra.data.Services;
using challenge.ibge.infra.data.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using LocalityService = challenge.ibge.infra.data.Services.LocalityService;

namespace challenge.ibge.infra.data;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ILocalityService, LocalityService>();
        services.AddScoped<ILocalityValidationService, LocalityValidationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEncryptService, EncryptService>();

        services.AddDbContext<MySqlDbContext>();
    }
}