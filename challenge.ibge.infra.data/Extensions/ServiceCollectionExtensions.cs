﻿using challenge.ibge.infra.data.Services;
using challenge.ibge.infra.data.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace challenge.ibge.infra.data.Extensions;

/// <summary>
/// Extension used to register services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ILocalityService, LocalityService>();
        services.AddScoped<ILocalityValidationService, LocalityValidationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEncryptPasswordService, EncryptPasswordService>();

        services.AddDbContext<MySqlDbContext>();
    }
}