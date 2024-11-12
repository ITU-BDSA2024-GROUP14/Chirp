using Microsoft.Playwright.NUnit;
using TestHelpers;

namespace EndToEndTests;

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