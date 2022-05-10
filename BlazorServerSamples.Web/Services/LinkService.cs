using BlazorServerSamples.Web.Data;
using BlazorServerSamples.Web.Domain;
using Microsoft.Extensions.Options;

namespace BlazorServerSamples.Web.Services;

public interface ILinkService
{
	List<Link> GetSitemapLinks();
	List<Link> GetHomeSidebarLinks();
	List<LinkBasic> GetAdminLinks();
	List<LinkBasic> GetDashboardLinks();
}

public class LinkService : ILinkService
{
	/*
	public IOptions<SukkotSettings> SukkotSettings { get; set; }
	public LinkService(IOptions<SukkotSettings> sukkotSettings)
	{
		SukkotSettings = sukkotSettings;
	}
	*/

	public List<Link> GetHomeSidebarLinks()
	{
		LinksFactory links = new LinksFactory();
		return links.GetLinks().Where(x => x.HomeSidebarUsage == true).ToList();
	}

	public List<Link> GetSitemapLinks()
	{
		LinksFactory links = new LinksFactory();
		return links.GetLinks().Where(x => x.SitemapUsage == true).ToList();
	}

	public List<LinkBasic> GetAdminLinks()
	{
		LinksFactory links = new LinksFactory();
		List<LinkBasic> feasts = new List<LinkBasic>();

		//return links.GetVideoProductionLinks().Union(links.GetEldersLinks()).Union(feasts).ToList();
		return links.GetDashboardLinks().ToList();
		}

	public List<LinkBasic> GetDashboardLinks()
	{
		LinksFactory links = new LinksFactory();
		return links.GetDashboardLinks().ToList();
	}
}
