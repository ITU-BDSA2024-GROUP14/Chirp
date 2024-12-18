using Microsoft.Playwright.NUnit;

namespace TestHelpers;

public abstract class SelfHostedPageTest : PageTest
{
    private static PlaywrightWebApplicationFactory<Program>? _webApplicationFactory;

    protected SelfHostedPageTest()
    {
        _webApplicationFactory ??= new PlaywrightWebApplicationFactory<Program>();
    }

    protected static IServiceProvider ServiceProvider => _webApplicationFactory!.Services;

    protected string GetServerAddress()
    {
        return _webApplicationFactory!.ServerAddress;
    }
}