namespace BlazorServerSamples.Web.Pages.MultiEditForm;

using Microsoft.Extensions.Logging;
using BlazorServerSamples.Web.Pages.MultiEditForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;


public interface IYouTubeFeedService
{
	//Task<List<YouTubeFeedModel>> GetModel(string url, int take);
	List<YouTubeFeedModel> GetModel();
}

public class YouTubeFeedService : IYouTubeFeedService
{
	#region Constructor and DI
	//private readonly IWeeklyVideosRepository db;
	private readonly ILogger Logger;

	public YouTubeFeedService(
		IWeeklyVideosRepository weeklyVideosRepository, ILogger<YouTubeFeedService> logger)
	{
		//db = weeklyVideosRepository;
		Logger = logger;
	}

	#endregion

	public List<YouTubeFeedModel> GetModel()
	{
		List<YouTubeFeedModel> l = new();

		l.Add(new YouTubeFeedModel()
		{
			Id = null,
			YouTubeId = "YouTubeId1",
			Title = "YouTube Id one",
			PublishDate = new DateTime(2022, 04, 29)
		});

		l.Add(new YouTubeFeedModel()
		{
			Id = null,
			YouTubeId = "YouTubeId2",
			Title = "YouTube Id two",
			PublishDate = new DateTime(2022, 05, 6)
		});

		l.Add(new YouTubeFeedModel()
		{
			Id = null,
			YouTubeId = "YouTubeId3",
			Title = "YouTube Id three",
			PublishDate = new DateTime(2022, 05, 13)
		});

		return l;
	}

	/*
	
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
	*/
}
