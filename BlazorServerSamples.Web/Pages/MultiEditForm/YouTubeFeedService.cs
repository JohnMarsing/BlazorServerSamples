namespace BlazorServerSamples.Web.Pages.MultiEditForm;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

public interface IYouTubeFeedService
{
	Task<List<YouTubeFeedModel>> GetModel(string url, int take);
}

public class YouTubeFeedService : IYouTubeFeedService
{
	#region Constructor and DI
	private readonly ILogger Logger;

	public YouTubeFeedService(
		ILogger<YouTubeFeedService> logger)
	{
		Logger = logger;
	}
	#endregion

	public async Task<List<YouTubeFeedModel>> GetModel(string url, int take)
	{
		Logger.LogDebug(string.Format("Inside {0}", nameof(YouTubeFeedService) + "!" + nameof(GetModel)));
		await Task.Delay(0);

		using var reader = XmlReader.Create(url);
		var feed = SyndicationFeed.Load(reader);
		List<SyndicationItem> si = new List<SyndicationItem>();
		si = feed.Items
			.OrderByDescending(x => x.PublishDate)
			.Take(take)
			.ToList();

		List<YouTubeFeedModel> l = new();
		if (si.Any())
		{
			foreach (var item in si)
			{
				l.Add(new YouTubeFeedModel()
				{
					Id = null,
					YouTubeId = item.Id.Replace("yt:video:", ""),
					Title = item.Title.Text,
					PublishDate = item.PublishDate
				});
			}
		}
		return l;
	}

}
