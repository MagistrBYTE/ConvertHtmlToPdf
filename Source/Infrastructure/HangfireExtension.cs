using Hangfire;
using Hangfire.PostgreSql;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using System;

namespace Convert.Infrastructure
{
	public static class HangfireExtension
	{
		public static IServiceCollection AddHangfireBlock(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHangfire((config) =>
			{
				config.UseSimpleAssemblyNameTypeSerializer();
				config.UseRecommendedSerializerSettings();
				config.UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddHangfireServer();

			//var attempts = configuration.GetSection("Hangfire").GetValue<int>("Attempts");
			//var delaysInSeconds = configuration.GetSection("Hangfire").GetValue<int>("DelaysInSeconds");
			//GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
			//{ Attempts = attempts, DelaysInSeconds = new[] { delaysInSeconds } });

			return (services);
		}
	}
}
