using Microsoft.Playwright;
using TestHelpers;

namespace UITests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class PlaywrightTests : SelfHostedPageTest<Program>
{
    [Test]
    public async Task CheepBoxIsVisiblePublic()
    {// Arrange
        var serverAddress = GetServerAddress();
        
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Expect(Page.GetByText("What's on your mind Helge? Share")).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task CheepBoxIsVisiblePrivate()
    {// Arrange
        var serverAddress = GetServerAddress();
        
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GotoAsync(serverAddress + "Helge");
        await Expect(Page.GetByText("What's on your mind Helge? Share")).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task CheepBoxNotVisibleNotLoggedIn()
    {
        // Arrange
        var serverAddress = GetServerAddress();
        
        //Act
        await Page.GotoAsync(serverAddress);
        await Expect(Page.GetByText("What's on your mind ropf@itu.dk? Share")).Not.ToBeVisibleAsync();
    }
    
    [Test]
    public async Task LongCheepLengthReduced()
    {
        // Arrange
        var serverAddress = GetServerAddress();
        
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message").FillAsync("I wish to write the longest cheep anyone has ever seen. Oh, I hope that everyone who sees this cheep, in its full length will be filled with joy at my wonderful long Cheep");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Helge I wish to write the longest cheep anyone has ever seen. Oh, I hope that everyone who sees this cheep, in its full length will be filled with joy at my wonderful");
        await Expect(Page.Locator("#messagelist")).Not.ToContainTextAsync("Helge I wish to write the longest cheep anyone has ever seen. Oh, I hope that everyone who sees this cheep, in its full length will be filled with joy at my wonderful long Cheep");
    }
    
    [Test]
    public async Task RegisterAndLogin()
    {
        var serverAddress = GetServerAddress();
        
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("joe@mama.com");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("W3are!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("W3are!");
        await Page.GetByLabel("Confirm Password").PressAsync("Tab");
        await Page.GetByPlaceholder("JohnDoe").FillAsync("Coolguy123");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("joe@mama.com");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByPlaceholder("password").FillAsync("W3are!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "logout [Coolguy123]" })).ToBeVisibleAsync();
        await Expect(Page.Locator("h3")).ToContainTextAsync("What's on your mind Coolguy123?");
    }
    
}