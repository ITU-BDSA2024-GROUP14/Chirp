using Microsoft.Extensions.Hosting;

namespace TestHelpers;
//https://medium.com/younited-tech-blog/end-to-end-test-a-blazor-app-with-playwright-part-3-48c0edeff4b6
// Relay the call to both test host and kestrel host.
public class CompositeHost(IHost testHost, IHost kestrelHost) : IHost
{
    public IServiceProvider Services => testHost.Services;

    public void Dispose()
    {
        testHost.Dispose();
        kestrelHost.Dispose();
    }

    public async Task StartAsync(
        CancellationToken cancellationToken = default)
    {
        await testHost.StartAsync(cancellationToken);
        await kestrelHost.StartAsync(cancellationToken);
    }

    public async Task StopAsync(
        CancellationToken cancellationToken = default)
    {
        await testHost.StopAsync(cancellationToken);
        await kestrelHost.StopAsync(cancellationToken);
    }
}