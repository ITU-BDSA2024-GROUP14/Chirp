using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data;
using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services;
using Chirp.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IChirpService, ChirpService>();
builder.Services.AddScoped<IDbInitializer, ProductionDbInitializer>();
builder.Services.AddTransient<IClaimsTransformation, AuthorClaimsTransformation>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<Author>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ChirpDBContext>();

//Setup github authentication
builder.Services.AddAuthentication().AddCookie().AddGitHub(o =>
{
    o.Scope.Add("user:email");
    o.Scope.Add("read:user");
    o.ClientId = builder.Configuration["authentication:github:clientId"]
                 ?? Environment.GetEnvironmentVariable("authentication__github__clientId")
                 ?? throw new InvalidOperationException("github:clientId secret not found");
    o.ClientSecret = builder.Configuration["authentication:github:clientSecret"]
                     ?? Environment.GetEnvironmentVariable("authentication__github__clientSecret")
                     ?? throw new InvalidOperationException("github:clientSecret secret not found");
    o.CallbackPath = "/signin-github";
});

//Validates scopes of services
builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateScopes = true;
    o.ValidateOnBuild = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Create a disposable service scope
using (var scope = app.Services.CreateScope())
{
    // From the scope, get an instance of our database context.
    // Through the `using` keyword, we make sure to dispose it after we are done.
    using var context = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();

    // Execute the migration from code.
    context.Database.Migrate();

    // Get the UserManager instance from the DI container
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Author>>();

    // Get the DbInitializer instance from the DI container
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

    //Seed database
    dbInitializer.Seed(userManager);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

// required per https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program;