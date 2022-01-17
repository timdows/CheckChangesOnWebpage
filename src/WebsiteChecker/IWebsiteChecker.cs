using System.Threading.Tasks;

namespace CheckChangesOnWebpage.WebsiteChecker
{
	public interface IWebsiteChecker
	{
		Task CheckWebsite();
	}
}
