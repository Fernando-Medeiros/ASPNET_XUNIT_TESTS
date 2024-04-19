using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using WebApi;

namespace Test.Setup;

internal class ServerFixture : IDisposable
{
    protected readonly TestServer _server;

    public readonly HttpClient Client;

    public ServerFixture(Action<IServiceCollection>? serviceOverride = null)
    {
        var _builder = new WebHostBuilder().UseStartup<Startup>();

        if (serviceOverride is Action<IServiceCollection>)
        {
            _builder.ConfigureTestServices(serviceOverride);
        }

        _server = new TestServer(_builder);

        Client = _server.CreateClient();
    }

    public void SaveEntityBeforeTest<T>(T entity) where T : class
    {
        using var scope = _server.Services.CreateScope();

        var _context = scope.ServiceProvider.GetService<DatabaseContext>();

        _context?.Add(entity);
        _context?.SaveChanges();
    }

    public virtual void Dispose()
    {
        Client.Dispose();
        _server.Dispose();
    }
}