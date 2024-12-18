using Microsoft.Playwright;
using TestHelpers;

namespace UITests;

[NonParallelizable]
[TestFixture]
public class EndToEndTest : SelfHostedPageTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        serverAddress = GetServerAddress();
    }

    private string serverAddress;

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
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "register" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("Test@example.dk");
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).FillAsync("Test1234!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test1234!");
        await Page.GetByPlaceholder("JohnDoe").ClickAsync();
        await Page.GetByPlaceholder("JohnDoe").FillAsync("Test user");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Register" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message").FillAsync("This is a test cheep");
        await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Test user's Timeline" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByText("Test user This is a test")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Listitem)).ToContainTextAsync("Test user This is a test cheep —");
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "public timeline" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Public Timeline" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Test user]" }))
            .ToBeVisibleAsync();
        await Expect(Page.Locator("li").Filter(new LocatorFilterOptions { HasText = "Test user This is a test" }))
            .ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Test user]" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Click here to Logout" }))
            .ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Click here to Logout" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = "Log out" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestThatOtherUserCanOnlySeeCheepOnPublicTimeline()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message").FillAsync("Hello, this is my test cheep");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Helge Hello, this is my test cheep —");
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Helge]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "public timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Helge Hello, this is my test cheep —");
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Helge]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Click here to Logout" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "public timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Helge Hello, this is my test cheep —");
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "my timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).Not.ToContainTextAsync("Helge Hello, this is my test cheep —");
    }

    [Test]
    public async Task TestThatYouCantLoginWithWrongInformation()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("Test@test");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Listitem)).ToContainTextAsync("Invalid login attempt.");
    }

    [Test]
    public async Task TestNextAndPreviosPageButtons()
    {
        await Page.GotoAsync(serverAddress);
        await Expect(Page.GetByText("1", new PageGetByTextOptions { Exact = true })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next Page" }))
            .ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next Page" }).ClickAsync();
        await Expect(Page.Locator("li")
                .Filter(new LocatorFilterOptions
                {
                    HasText = "Jacqualine Gilcoine I sat down at the moor-gate where he was."
                }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByText("2", new PageGetByTextOptions { Exact = true })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Previous Page" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next Page" }))
            .ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next Page" }).ClickAsync();
        await Expect(Page.GetByText("3", new PageGetByTextOptions { Exact = true })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Previous Page" }).ClickAsync();
        await Expect(Page.GetByText("2", new PageGetByTextOptions { Exact = true })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Previous Page" }).ClickAsync();
        await Expect(Page.Locator("li").Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine Sometimes" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByText("1", new PageGetByTextOptions { Exact = true })).ToBeVisibleAsync();
    }

    [Test]
    public async Task ForgetMe_DeletesAllUserData()
    {
        // Arrange
        // create a new user
        var email = "throwaway@user.com";
        var name = "ThrowawayUser";
        var password = "W3are!";

        await Page.GotoAsync(serverAddress + "Identity/Account/Register");
        await Page.GetByPlaceholder("name@example.com").FillAsync(email);
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).FillAsync(password);
        await Page.GetByLabel("Confirm Password").FillAsync(password);
        await Page.GetByPlaceholder("JohnDoe").FillAsync(name);
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Register" }).ClickAsync();

        // post 2 cheeps
        await Page.Locator("#Message").FillAsync("MY CPR NUMBER IS 220253-6379");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" }).ClickAsync();
        await Page.Locator("#Message").FillAsync("OOPS wrong place");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" }).ClickAsync();
        await Expect(Page.GetByText("MY CPR NUMBER IS 220253-6379")).ToBeVisibleAsync();
        await Expect(Page.GetByText("OOPS wrong place")).ToBeVisibleAsync();

        // Act
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "about me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Forget me!" }).ClickAsync();

        // Assert
        //  that the user cannot be used
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("throwaway@user.com");
        await Page.GetByPlaceholder("password").FillAsync("W3are!");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Expect(Page.GetByText("Invalid login attempt.")).ToBeVisibleAsync();

        // that the user is logged out
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" })).ToBeVisibleAsync();

        // that the cheeps are gone
        await Page.GotoAsync(serverAddress + "ThrowawayUser");
        await Expect(Page.GetByText("There are no cheeps so far.")).ToBeVisibleAsync();
    }
}