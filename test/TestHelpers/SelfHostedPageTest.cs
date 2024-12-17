using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace TestHelpers;

public abstract class SelfHostedPageTest : PageTest
{
    private readonly PlaywrightWebApplicationFactory<Program>? _webApplicationFactory;
    protected IServiceProvider ServiceProvider => _webApplicationFactory!.Services;

    protected SelfHostedPageTest()
    {
        _webApplicationFactory ??= new PlaywrightWebApplicationFactory<Program>();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _webApplicationFactory?.Dispose();
    }

    protected string GetServerAddress()
    {
        return _webApplicationFactory!.ServerAddress;
    }
}