using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TestHelpers;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace EndToEndTests;

public class EndToEndTest
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public EndToEndTest(CustomWebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task TestLoginViaUserName()
        {
            await Page.GotoAsync("http://localhost:5273/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
            await Page.GetByPlaceholder("name@example.com").ClickAsync();
            await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
            await Page.GetByPlaceholder("password").ClickAsync();
            await Page.GetByPlaceholder("password").ClickAsync();
            await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
            await Page.GetByPlaceholder("password").PressAsync("Enter");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "logout [ropf@itu.dk]" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        }
    }
}