using CheckChangesOnWebpage.WebsiteCheker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace CheckChangesOnWebpage
{
	class Program
	{

		public static async Task Main(string[] args)
		{
			using var host = CreateHostBuilder(args).Build();

			await DoWork(host.Services);
		}

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) =>
					services.AddTransient<IWebsiteChecker, WebsiteChecker>());
		}

		public static async Task DoWork(IServiceProvider services)
		{
			using var serviceScope = services.CreateScope();
			var provider = serviceScope.ServiceProvider;

			var websiteChecker = provider.GetRequiredService<IWebsiteChecker>();

			await websiteChecker.CheckWebsite();
		}
	}
}
