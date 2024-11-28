using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Prover.Entrevista.Core.Common.Behaviors;
using System.Reflection;

namespace Prover.Entrevista.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}