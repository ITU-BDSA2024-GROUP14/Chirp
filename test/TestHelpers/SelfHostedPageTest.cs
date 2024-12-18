using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace TestHelpers;

public abstract class SelfHostedPageTest : PageTest
{
    private static PlaywrightWebApplicationFactory<Program>? _webApplicationFactory;
    protected static IServiceProvider ServiceProvider => _webApplicationFactory!.Services;

    protected SelfHostedPageTest()
    {
        _webApplicationFactory ??= new PlaywrightWebApplicationFactory<Program>();
    }

    protected string GetServerAddress()
    {
        return _webApplicationFactory!.ServerAddress;
    }
}