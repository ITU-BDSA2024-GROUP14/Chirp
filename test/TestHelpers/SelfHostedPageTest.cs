using Microsoft.Playwright.NUnit;

namespace TestHelpers;

public abstract class SelfHostedPageTest : PageTest
{
    private PlaywrightWebApplicationFactory<Program>? s_webApplicationFactory;
    protected IServiceProvider ServiceProvider => s_webApplicationFactory!.Services;

    protected SelfHostedPageTest()
    {
        s_webApplicationFactory ??= new PlaywrightWebApplicationFactory<Program>();
    }

    protected string GetServerAddress()
    {
        return s_webApplicationFactory!.ServerAddress;
    }

    protected void ResetWebApplicationFactory()
    {
        s_webApplicationFactory = new PlaywrightWebApplicationFactory<Program>();
    }
}