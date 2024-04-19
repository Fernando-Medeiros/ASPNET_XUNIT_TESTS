using WebApi.Infrastructure;

namespace WebApi;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        services.AddAuthenticationSchemes(configuration);
        services.AddAuthorizationPolicies();

        services.AddEndpointsApiExplorer();

        services.AddRouting();

        // Services Extension
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        // UseMiddleware
        // Endpoints
    }
}