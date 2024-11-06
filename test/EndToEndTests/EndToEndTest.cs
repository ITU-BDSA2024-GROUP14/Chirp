using Microsoft.Playwright;
using TestHelpers;
using Microsoft.Playwright.NUnit;

namespace EndToEndTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EndToEndTest : PageTest
{
    private string _serverAddress;

    [OneTimeSetUp]
    public void Setup()
    {
        var fixture = new CustomWebApplicationFactory<Program>();
        _serverAddress = fixture.ServerAddress;
    }

    [Test]
    public async Task TestLoginViaUserName()
    {
        await Page.GotoAsync(_serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByPlaceholder("password").PressAsync("Enter");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "my timeline" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [ropf@itu.dk]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Click here to Logout" }).ClickAsync();
    }
}