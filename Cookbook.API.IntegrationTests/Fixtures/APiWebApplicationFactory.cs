using Cookbook.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Xunit;

namespace Cookbook.API.IntegrationTests.Fixtures;

public class APiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
			.WithImage("postgres")
			.WithDatabase("cookbooktest")
			.WithUsername("postgres")
			.WithPassword("password")
			.WithCleanUp(true)
			.Build();

	public IConfiguration Configuration { get; private set; } = null!;

	public async Task InitializeAsync()
	{
		await _dbContainer.StartAsync();

		using var scope = Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<CookbookContext>();
		await context.Database.EnsureDeletedAsync();
		await context.Database.EnsureCreatedAsync();
	}

	public new async Task DisposeAsync()
	{
		await _dbContainer.DisposeAsync();
		await base.DisposeAsync();
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureAppConfiguration(config =>
		{
			Configuration = new ConfigurationBuilder()
					.AddJsonFile("appsettings.Integration.json")
					.Build();
			config.AddConfiguration(Configuration);
		});

		builder.ConfigureTestServices(services =>
		{
			services.RemoveAll<DbContextOptions<CookbookContext>>();
			services.RemoveAll<CookbookContext>();

			services.AddDbContext<CookbookContext>(options => options
					.UseNpgsql(_dbContainer.GetConnectionString() + ";Include Error Detail=true")
					.UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
					{
						var context = (CookbookContext)dbContext;
						await context.SeedAsync(context, cancellationToken);
					}));
		});
	}
}