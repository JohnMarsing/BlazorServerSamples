namespace BlazorServerSamples.Web.Domain;

public class Link
{
	public string Index { get; set; } = String.Empty;
	public string Title { get; set; } = String.Empty;
	public string Icon { get; set; } = String.Empty;

	public bool SitemapUsage { get; set; }
	public bool HomeSidebarUsage { get; set; } 
}
