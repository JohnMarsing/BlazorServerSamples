using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using BlazorServerSamples.Web.Domain;
using Microsoft.Extensions.Logging;

namespace BlazorServerSamples.Web.Pages;

public partial class NavigationSearch
{
	[Inject]
	public Services.ILinkService? LinkService { get; set; }

	[Inject]
	NavigationManager? NavigationManager { get; set; }

	//[Inject] public ILogger<NavigationSearch> Logger { get; set; }

	protected string? item;
	public string? Msg { get; set; }

	void SearchClicked()
	{
		//string inside = nameof(NavigationSearch) + "!" + nameof(SearchClicked);
		if (String.IsNullOrEmpty(item))
		{
			//Logger.LogDebug(string.Format("Inside {0}; item is null", inside));
			Msg = "item is null";
		}
		else
		{
			Link? link;
			link = LinkService.GetSitemapLinks().Where(w => w.Title.ToLower() == item.ToLower()).FirstOrDefault();
			if (link is not null)
			{
				//Logger.LogInformation(string.Format("Inside {0}; item:{1} Navigating to {2}; ", inside, item, link.Index));
				NavigationManager.NavigateTo($"{link.Index}",true);
			}
			else
			{
				//Logger.LogDebug(string.Format("Inside {0}; link is null", inside));
				Msg = "link is null";
			}

		}
	}



	private IEnumerable<Link> Links;
	protected override void OnInitialized()
	{
		Links = LinkService!.GetSitemapLinks();
	}
	public string SearchText = "";
	List<Link> FilteredLinks => Links.Where(w => w.Title.ToLower().Contains(SearchText.ToLower())).ToList();

}



