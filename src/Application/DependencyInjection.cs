using ExpenseTracker.Application.Authentication.Commands.Register;
using ExpenseTracker.Application.Common.Behaviors;
using ExpenseTracker.Application.Common.Mapping;
using ExpenseTracker.Application.Transactions.Commands.CreateTransaction;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExpenseTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddMapping();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
        return services;
    }


    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(TransactionMappingConfig).Assembly);

        return services;
    }
}
