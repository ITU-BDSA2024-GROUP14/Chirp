using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace TestHelpers;

public abstract class SelfHostedPageTest : PageTest
{
    public static PlaywrightWebApplicationFactory<Program>? WebApplicationFactory;

    public SelfHostedPageTest()
    {
        WebApplicationFactory ??= new PlaywrightWebApplicationFactory<Program>();
    }

    protected string GetServerAddress()
    {
        return WebApplicationFactory!.ServerAddress;
    }
}