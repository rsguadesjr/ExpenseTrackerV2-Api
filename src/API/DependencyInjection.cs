using ExpenseTracker.API.Converters;
using ExpenseTracker.API.Errors;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection;

namespace ExpenseTracker.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
                    });
            services.AddSwaggerGen();
            services.AddSingleton<ProblemDetailsFactory, ExpenseTrackerProblemDetailsFactory>();
            services.AddMapping();
            return services;
        }


        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }


    }
}
