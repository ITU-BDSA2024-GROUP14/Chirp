using Microsoft.Playwright;
using TestHelpers;
using Microsoft.Playwright.NUnit;

namespace EndToEndTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EndToEndTest : SelfHostedPageTest<Program>
{
    private string serverAddress;

    [SetUp]
    public void Setup()
    {
        serverAddress = GetServerAddress();
    }

    [Test]
    public async Task TestLoginViaUserName()
    {
        
        
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByPlaceholder("password").PressAsync("Enter");
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "my timeline" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Helge]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Click here to Logout" }).ClickAsync();
    }
    
    [Test]
    public async Task TestCreateUserAndPostCheep()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("Test@example.dk");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test1234!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test1234!");
        await Page.GetByPlaceholder("JohnDoe").ClickAsync();
        await Page.GetByPlaceholder("JohnDoe").FillAsync("Test user");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("Test@example.dk");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByPlaceholder("password").FillAsync("Test1234!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message").FillAsync("This is a test cheep");
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Share" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Test user's Timeline" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Test user This is a test")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Paragraph)).ToContainTextAsync("Test user This is a test cheep —");
        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "logout [Test user]" })).ToBeVisibleAsync();
        await Expect(Page.Locator("li").Filter(new() { HasText = "Test user This is a test" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Test user]" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log out" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "login" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task MyTest()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message").FillAsync("Hello, this is my test cheep");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Helge Hello, this is my test cheep —");
        await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Helge]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Helge Hello, this is my test cheep —");
        await Page.GetByRole(AriaRole.Link, new() { Name = "logout [Helge]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Helge Hello, this is my test cheep —");
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).Not.ToContainTextAsync("Helge Hello, this is my test cheep —");
    }

}