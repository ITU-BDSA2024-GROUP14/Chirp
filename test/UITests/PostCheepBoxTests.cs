using Microsoft.Playwright;
using TestHelpers;

namespace UITests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : SelfHostedPageTest<Program>
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
}