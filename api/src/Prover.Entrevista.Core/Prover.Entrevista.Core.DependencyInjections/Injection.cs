using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prover.Entrevista.Core.Application.Interfaces;
using Prover.Entrevista.Core.Application.Services;
using Prover.Entrevista.Core.Application.Services.Authentication;
using Prover.Entrevista.Core.Domain.Interfaces.Repositories;
using Prover.Entrevista.Core.Infrastructure.Context;
using Prover.Entrevista.Core.Infrastructure.Interfaces.Context;
using Prover.Entrevista.Core.Infrastructure.Repositories;

namespace Prover.Entrevista.Core.DependencyInjections;

public static class Injection
{
    public static void DependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.InjectionContext();
        services.InjectionServices();
        services.InjectionRepositories();
    }

    public static void InjectionServices(this IServiceCollection services)
    {
        services.TryAddScoped<IUserService, UserService>();
        services.TryAddScoped<IProductService, ProductService>();

        services.TryAddScoped<IAuthService, AuthService>();
        services.TryAddScoped<IJwtTokenService, JwtTokenService>();
    }

    public static void InjectionContext(this IServiceCollection services)
    {
        services.TryAddScoped<IDbContext, DbContext>();
    }

    public static void InjectionRepositories(this IServiceCollection services)
    {
        services.TryAddScoped<IUserRepository, UserRepository>();
        services.TryAddScoped<IProductRepository, ProductRepository>();
    }
}