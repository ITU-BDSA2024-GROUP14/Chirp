using Microsoft.Playwright.NUnit;

namespace TestHelpers;

public abstract class SelfHostedPageTest<TEntryPoint> : PageTest where TEntryPoint : class
{
    private readonly PlaywrightWebApplicationFactory<TEntryPoint> _webApplicationFactory;

    public SelfHostedPageTest()
    {
        _webApplicationFactory = new PlaywrightWebApplicationFactory<TEntryPoint>();
    }

    protected string GetServerAddress()
    {
        return _webApplicationFactory.ServerAddress;
    }
}