using PushBulletNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckChangesOnWebpage.WebsiteChecker
{
	public class WebsiteChecker : IWebsiteChecker
	{
		private List<string> _lastEntries = new();

		public WebsiteChecker()
		{

		}

		public async Task CheckWebsite()
		{
			using var client = new HttpClient();
			client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

			while(true)
			{
				try
				{
					// Get a string with the complete website
					var content = await client.GetStringAsync("https-website-to-check");

					// Search for specific lines in the website
					var split = content.Split('\n');
					var currentEntries = split
						.Where(item => item.Contains("search-string", StringComparison.OrdinalIgnoreCase))
						.ToList();

					// Compare the previous run with the current entries found
					var firstNotSecond = currentEntries.Except(_lastEntries).ToList();
					var secondNotFirst = _lastEntries.Except(currentEntries).ToList();

					// If any of the information changed, send out a notification
					if (firstNotSecond.Any() || secondNotFirst.Any())
					{
						_lastEntries = currentEntries;

						var pushBulletClient = new PushBulletClient("secret-code");
						await pushBulletClient.PushAsync("WebsiteCheker", $"{DateTime.Now} content is different", "receiver");

						Console.WriteLine($"{DateTime.Now} content is different");
					}
					else
					{
						Console.WriteLine($"{DateTime.Now} content is equal");
					}
				}
				catch (Exception excep)
				{
					Console.WriteLine(excep.Message);
				}

				await Task.Delay(1000 * 60);
			}
		}
	}
}
