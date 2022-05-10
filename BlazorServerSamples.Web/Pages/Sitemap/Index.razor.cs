using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BlazorServerSamples.Web.Domain;

namespace BlazorServerSamples.Web.Pages.Sitemap
{
    public partial class Index
    {
		[Inject]
		public Services.ILinkService LinkService { get; set; }

		private IEnumerable<Link> SitemapLinks;
		protected override void OnInitialized()
		{
			base.OnInitialized();
			SitemapLinks = LinkService.GetSitemapLinks();
		}
	}
}