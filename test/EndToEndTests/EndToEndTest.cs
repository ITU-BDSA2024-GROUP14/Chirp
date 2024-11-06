using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TestHelpers;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace EndToEndTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EndToEndTest : PageTest
{
    private WebApplicationFactory<Program> _fixture;

    private HttpClient _client;


    [OneTimeTearDown]
    public void Cleanup()
    {
        _client.Dispose();
    }

    public EndToEndTest(CustomWebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Test]
    public async Task TestLoginViaUserName()
    {
        await Page.GotoAsync("http://localhost:5273/");
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