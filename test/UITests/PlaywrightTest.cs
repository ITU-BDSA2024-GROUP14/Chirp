using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using TestHelpers;

namespace UITests;

[NonParallelizable]
[TestFixture]
public class PlaywrightTest : SelfHostedPageTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        serverAddress = GetServerAddress();
    }

    private string serverAddress;

    [Test]
    public async Task XSSAttackResiliancy()
    {
        var dialogAppeared = false;
        //Act
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message").FillAsync("\"><script>alert('XSS Test');</script>");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" }).ClickAsync();

        Page.Dialog += (_, _) =>
        {
            dialogAppeared = true;
        };


        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Adrian \"><script>alert('XSS Test');</script>");
        Assert.That(dialogAppeared, Is.False);
    }

    [Test]
    public async Task SQLInjectionAttackTest()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.Locator("#Message").ClickAsync();
        await Page.Locator("#Message").FillAsync("Robert'); DROP TABLE AspNetUsers;--");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Share" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "public timeline" }).ClickAsync();
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
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();

        // Follow Jacqualine
        await Page.Locator("li").Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine" })
            .GetByRole(AriaRole.Button).First
            .ClickAsync();

        // Expect Jacqualine to be visible on personal timeline, while logged in
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "my timeline" }).ClickAsync();
        await Expect(Page.Locator("p").Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine" })
                .GetByRole(AriaRole.Link).First)
            .ToBeVisibleAsync();

        // Expect Jacqualine (not recheeps) to not be visible on Adrians timeline, when not logged in
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Adrian]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Click here to Logout" }).ClickAsync();
        await Page.GotoAsync(serverAddress + "Adrian");
        await Expect(Page.Locator("li")
            .Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine", HasNotText = "re-cheeped" })
            .GetByRole(AriaRole.Link)).ToHaveCountAsync(0);
    }

    [Test]
    public async Task UnfollowTest()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();

        // unfollow
        await Page.Locator("li")
            .Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine" })
            .Filter(new LocatorFilterOptions { HasText = "Unfollow" })
            .GetByRole(AriaRole.Button).First.ClickAsync();

        // go to own timeline
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "my timeline" }).ClickAsync();

        // expect Jacqualine's original cheeps to not be visible
        await Expect(Page.Locator("#messagelist")
                .Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine", HasNotText = "re-cheeped" }))
            .ToHaveCountAsync(0);
    }

    [Test]
    public async Task PersonalDataPage_ShowsAllData()
    {
        Author user;
        using (var scope = ServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();
            user = dbContext.Users
                .Include(u => u.Cheeps)
                .Include(u => u.Following)
                .First(u => u.UserName == "ropf@itu.dk");
        }

        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("ropf@itu.dk");
        await Page.GetByPlaceholder("password").FillAsync("LetM31n!");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.GotoAsync(serverAddress + "PersonalData");

        await Expect(Page.Locator(".body").GetByText(user.DisplayName, new LocatorGetByTextOptions { Exact = true }))
            .ToBeVisibleAsync();
        await Expect(Page.Locator(".body").GetByText(user.Email, new LocatorGetByTextOptions { Exact = true }))
            .ToBeVisibleAsync();

        foreach (var follow in user.Following)
        {
            await Expect(Page.Locator("#followList")).ToContainTextAsync(follow.DisplayName);
        }

        foreach (var cheep in user.Cheeps)
        {
            await Expect(Page.Locator("#messagelist")).ToContainTextAsync(cheep.GetText());
        }
    }

    [Test]
    public async Task PaginationButtons()
    {
        await Page.GotoAsync(serverAddress);
        await Expect(Page.Locator("#messagelist > li").First).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next Page" }).ClickAsync();

        await Expect(Page.Locator("#messagelist > li").First).ToBeVisibleAsync();
    }

    [Test]
    public async Task ReCheepShown()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Expect(Page.Locator("#messagelist"))
            .ToContainTextAsync("Jacqualine Gilcoine Starbuck now is what we hear the worst.");
        await Expect(Page.Locator("li").Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine Starbuck" })
            .GetByRole(AriaRole.Button).Nth(1)).ToBeVisibleAsync();
        await Page.Locator("li").Filter(new LocatorFilterOptions { HasText = "Jacqualine Gilcoine Starbuck" })
            .GetByRole(AriaRole.Button)
            .Nth(1).ClickAsync();
        await Expect(Page.Locator("#messagelist"))
            .ToContainTextAsync("Adrian re-cheeped Jacqualine Gilcoine Starbuck now is what we hear the worst.");
    }

    [Test]
    public async Task TestRecheepOthers()
    {
        await Page.GotoAsync(serverAddress);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("adho@itu.dk");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("M32Want_Access");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.Locator("li")
            .Filter(
                new LocatorFilterOptions { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst." })
            .GetByRole(AriaRole.Button).Nth(1).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "logout [Adrian]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Click here to Logout" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "register" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("2@2");
        await Page.GetByPlaceholder("name@example.com").PressAsync("Tab");
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).FillAsync("M32Want_Access");
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).PressAsync("Tab");
        await Page.GetByLabel("Confirm Password").FillAsync("M32Want_Access");
        await Page.GetByPlaceholder("JohnDoe").ClickAsync();
        await Page.GetByPlaceholder("JohnDoe").FillAsync("testuser");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Register" }).ClickAsync();
        await Page.Locator("button").First.ClickAsync(); //Follow
        await Page.Locator("li:nth-child(2) > div > .reCheep > form > button").ClickAsync(); //Recheep
        await Expect(Page.Locator("li").Filter(new LocatorFilterOptions { HasText = "testuser re-cheeped" }).First)
            .ToBeVisibleAsync();
        await Expect(Page.Locator("li").Filter(new LocatorFilterOptions { HasText = "Adrian re-cheeped" }).First)
            .ToBeVisibleAsync();
    }
}