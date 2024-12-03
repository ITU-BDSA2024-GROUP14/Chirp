using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace TestHelpers;

public abstract class SelfHostedPageTest : PageTest
{
    private static PlaywrightWebApplicationFactory<Program>? s_webApplicationFactory;
    protected static IServiceProvider ServiceProvider => s_webApplicationFactory!.Services;

    protected SelfHostedPageTest()
    {
        s_webApplicationFactory ??= new PlaywrightWebApplicationFactory<Program>();
    }

    protected string GetServerAddress()
    {
        return s_webApplicationFactory!.ServerAddress;
    }

    public void ResetDatabase()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        var userManager = scope.ServiceProvider.GetService<UserManager<Author>>();
        initializer.Seed(userManager);
    }
}