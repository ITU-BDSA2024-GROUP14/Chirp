namespace EndToEndTests;

using Microsoft.Playwright.NUnit;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task MyTest()
    {
        await Page.GotoAsync("https://demo.playwright.dev/todomvc/#/");
        await Page.GetByPlaceholder("What needs to be done?").ClickAsync();
        await Page.GetByPlaceholder("What needs to be done?").FillAsync("I write!");
        await Page.GetByPlaceholder("What needs to be done?").PressAsync("Enter");
        await Expect(Page.GetByTestId("todo-title")).ToContainTextAsync("I write!");
    }
}