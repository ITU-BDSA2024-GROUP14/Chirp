using Microsoft.Playwright;
using TestHelpers;

namespace UITests;

[NonParallelizable]
[TestFixture]
public class PlaywrightTest : SelfHostedPageTest
{
    private string serverAddress;

    [OneTimeSetUp]
    public void Setup()
    {
        serverAddress = GetServerAddress();
    }

    [Test]
    public async Task CheepBoxIsVisiblePublic()
    {
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Expect(Page.GetByText("What's on your mind Helge? Share")).ToBeVisibleAsync();
    }

    [Test]
    public async Task CheepBoxIsVisiblePrivate()
    {
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.GotoAsync(serverAddress + "Helge");
        await Expect(Page.GetByText("What's on your mind Helge? Share")).ToBeVisibleAsync();
    }

    [Test]
    public async Task CheepBoxNotVisibleNotLoggedIn()
    {
        //Act
        await Page.GotoAsync(serverAddress);
        await Expect(Page.GetByText("What's on your mind ropf@itu.dk? Share")).Not.ToBeVisibleAsync();
    }

    [Test]
    public async Task LongCheepLengthReduced()
    {
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message")
            .FillAsync(
                "I wish to write the longest cheep anyone has ever seen. Oh, I hope that everyone who sees this cheep, in its full length will be filled with joy at my wonderful long Cheep");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync(
            "Helge I wish to write the longest cheep anyone has ever seen. Oh, I hope that everyone who sees this cheep, in its full length will be filled with joy at my wonderful");
        await Expect(Page.Locator("#messagelist")).Not.ToContainTextAsync(
            "Helge I wish to write the longest cheep anyone has ever seen. Oh, I hope that everyone who sees this cheep, in its full length will be filled with joy at my wonderful long Cheep");
    }

    [Test]
    public async Task RegisterAndLogin()
    {
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "register" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("joe@mama.com");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).FillAsync("W3are!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("W3are!");
        await Page.GetByLabel("Confirm Password").PressAsync("Tab");
        await Page.GetByPlaceholder("JohnDoe").FillAsync("Coolguy123");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Register" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Coolguy123]" }))
            .ToBeVisibleAsync();
        await Expect(Page.Locator("h3")).ToContainTextAsync("What's on your mind Coolguy123?");
    }
    
    [Test]
    public async Task TestFollow()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.Locator("li").Filter(new() { HasText = "Mellie Yost But what" }).GetByRole(AriaRole.Button).First.ClickAsync();
        await Expect(Page.GetByText("Jacqualine Gilcoine Starbuck")).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        await Expect(Page.GetByText("Mellie Yost But what was")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Jacqualine Gilcoine Starbuck")).Not.ToBeVisibleAsync();
        await Expect(Page.GetByText("Adrian Hej, velkommen til kurset.")).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Adrian]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await Page.GotoAsync(serverAddress +"Adrian");
        await Expect(Page.GetByText("Adrian Hej, velkommen til kurset.")).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task UnfollowTest()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst. — 01/08/23" }).GetByRole(AriaRole.Button).First.ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        await Expect(Page.GetByText("Jacqualine Gilcoine Starbuck")).ToBeVisibleAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Starbuck now is what we hear the worst.");
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst. — 01/08/23" }).GetByRole(AriaRole.Button).First.ClickAsync();
        await Expect(Page.GetByText("Jacqualine Gilcoine Starbuck")).Not.ToBeVisibleAsync();
        await Expect(Page.Locator("#messagelist")).Not.ToContainTextAsync("Starbuck now is what we hear the worst.");
    }
}