using ExpenseTracker.API;
using ExpenseTracker.Application;
using ExpenseTracker.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
    });

    var allowedOriginsConfig = builder.Configuration.GetSection("Cors:AllowedOrigins");
    var allowedMethodsConfig = builder.Configuration.GetSection("Cors:AllowedMethods");
    var allowedOrigins = allowedOriginsConfig.Get<string[]>() ?? [];
    var allowedMethods = allowedMethodsConfig.Get<string[]>() ?? [];
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
                          policy =>
                          {
                              policy.WithOrigins(allowedOrigins)
                                    .WithMethods(allowedMethods)
                                    .AllowAnyHeader();
                          });
    });
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("CorsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.MapControllers();
}
app.Run();