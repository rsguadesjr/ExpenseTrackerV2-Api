using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Infrastructure.Authentication;
using ExpenseTracker.Infrastructure.Persistence;
using ExpenseTracker.Infrastructure.Persistence.Repositories;
using ExpenseTracker.Infrastructure.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IdentityModel.Tokens.Jwt;

namespace ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        var firebaseConfig = configuration.GetValue<string>("Firebase:Config");
        //var jwtSettings = configuration.GetValue<JwtSettings>("JwtSettings");

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(firebaseConfig)
            });
        }

        services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration["JwtSettings:Issuer"];
                    options.Audience = configuration["JwtSettings:Audience"];
                    options.TokenValidationParameters.ValidIssuer = configuration["JwtSettings:Issuer"];

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = async ctx =>
                        {
                            var putBreakpointHere = true;
                            var exceptionMessage = ctx.Exception;
                        },
                    };
                });

        // inject IConfiguration
        services.AddSingleton(configuration);

        services.AddScoped<IExpenseTrackerDbContext>(c => c.GetRequiredService<ExpenseTrackerDbContext>());
        services.AddDbContext<ExpenseTrackerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddScoped<IRequestContext, RequestContextService>();

        return services;
    }
}
