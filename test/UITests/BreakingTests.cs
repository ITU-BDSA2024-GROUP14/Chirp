using Microsoft.Playwright;
using TestHelpers;

namespace UITests;

[NonParallelizable]
[TestFixture]
public class BreakingTests : SelfHostedPageTest
{
    private string serverAddress;

    [OneTimeSetUp]
    public void Setup()
    {
        serverAddress = GetServerAddress();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ResetWebApplicationFactory();
    }
    
    [Test]
    public async Task ReCheepShown()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Jacqualine Gilcoine Starbuck now is what we hear the worst.");
        await Expect(Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).Nth(1).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Adrian re-cheeped Jacqualine Gilcoine Starbuck now is what we hear the worst.");
    }
    
    [Test]
    public async Task TestRecheepOthers()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst." }).GetByRole(AriaRole.Button).Nth(1).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Adrian]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("2@2");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("M32Want_Access");
        await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
        await Page.GetByLabel("Confirm Password").FillAsync("M32Want_Access");
        await Page.GetByPlaceholder("JohnDoe").ClickAsync();
        await Page.GetByPlaceholder("JohnDoe").FillAsync("testuser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.Locator("button").First.ClickAsync(); //Follow
        await Page.Locator("li:nth-child(2) > div > .reCheep > form > button").ClickAsync(); //Recheep
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("testuser re-cheeped Jacqualine Gilcoine Starbuck now is what we hear the worst.");
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Adrian re-cheeped Jacqualine Gilcoine Starbuck now is what we hear the worst.");
    }
}