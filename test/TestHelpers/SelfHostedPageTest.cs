using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace TestHelpers;

public abstract class SelfHostedPageTest : PageTest
{
    private static PlaywrightWebApplicationFactory<Program>? s_webApplicationFactory;

    protected SelfHostedPageTest()
    {
        s_webApplicationFactory ??= new PlaywrightWebApplicationFactory<Program>();
    }

    protected string GetServerAddress()
    {
        return s_webApplicationFactory!.ServerAddress;
    }
}