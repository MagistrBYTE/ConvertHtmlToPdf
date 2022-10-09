using Microsoft.EntityFrameworkCore;

namespace Convert.DataLayer
{
	public static class FileConvertedDbContextExtension
	{
		public static IServiceCollection AddFileConvertedDb(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddPooledDbContextFactory<FileConvertedDbContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddScoped<FileConvertedDbContextFactory>();

			return services;
		}

		public static void InitFileConvertedDb(this WebApplication app, Action<FileConvertedDbContext> init_action)
		{
			using var service_scope = app.Services.CreateScope();
			var dbContext = service_scope
				.ServiceProvider
				.GetService<IDbContextFactory<FileConvertedDbContext>>()!
				.CreateDbContext();

			init_action.Invoke(dbContext);
		}
	}
}
