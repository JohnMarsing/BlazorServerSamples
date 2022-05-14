using BlazorServerSamples.Web.Domain;
using BlazorServerSamples.Web.Links;

namespace BlazorServerSamples.Web.Data;

public interface ILinksFactory
{
	List<Link> GetLinks();
	List<LinkBasic> GetDashboardLinks();
}

public class LinksFactory : ILinksFactory
{

	public List<LinkBasic> GetDashboardLinks()
	{
		return new List<LinkBasic>
			{
				new LinkBasic {Index = "/Admin/Dashboard/Index", Title = "Dashboard", Icon = "fas fa-tachometer-alt", },
				new LinkBasic {Index = "/Admin/Dashboard/Dump", Title = "Dump", Icon = "fas fa-truck-monster", },
				new LinkBasic {Index = "/Admin/Dashboard/FontAwesome", Title = "FontAwesome", Icon = "fab fa-font-awesome-flag", },
				new LinkBasic {Index = "/Admin/Dashboard/PerformanceCompliance", Title = "Performance Compliance", Icon = "fas fa-rocket", },
				new LinkBasic {Index = "/Admin/Dashboard/Routes", Title = "Routes", Icon = "fas fa-route", },
				new LinkBasic {Index = "/Admin/Dashboard/ThrowException", Title = "Throw Exception", Icon = "fas fa-bomb", },
 			};
	}
	
		public List<Link> GetLinks()
		{
				return new List<Link>
			{
					new Link
				{
					Index = About.Index,
					Title = About.Title,
					Icon = About.Icon,
					HomeSidebarUsage=true,
					SitemapUsage=true
				},
					new Link
				{
					Index = MultiEditForm.Index,
					Title = MultiEditForm.Title,
					Icon = MultiEditForm.Icon,
					HomeSidebarUsage=true,
					SitemapUsage=true
				},
				new Link
				{
					Index = Sitemap.Index,
					Title = Sitemap.Title,
					Icon = Sitemap.Icon,
					HomeSidebarUsage=true,
					SitemapUsage=false // Don't show sitemap link on sitemap page
				}


			};
		}

}
