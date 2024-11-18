using System.Data.Common;
using Chirp.Infrastructure.Data;
using Chirp.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestHelpers;

//Class based on code from .net documentation https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#customize-webapplicationfactory
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ChirpDBContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<ChirpDBContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });

            services.RemoveAll<IDbInitializer>();
            services.AddScoped<IDbInitializer, TestDbInitializer>();
        });

        builder.UseEnvironment("Development");
    }
}