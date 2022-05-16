namespace BlazorServerSamples.Web;

public static class SocialMedia
{
	public static class YouTube
	{
		private const string _channelId = "UCz_q3-dBtU_sSbEojRP57OQ";
		private const string _baseFeedUrl = "https://www.youtube.com/feeds/videos.xml?channel_id=";
		private const string _baseNormalUrl = "https://www.youtube.com/channel/";
		private const string _baseSearchUrl = "https://www.youtube.com/results?search_query=living+messiah";
		public static string YouTubeFeed()
		{
			return _baseFeedUrl + _channelId;
		}

		public static string YouTubeNormal()
		{
			return _baseNormalUrl + _channelId;
		}

		public static string YouTubeSearch()
		{
			return _baseSearchUrl;
		}

	}
}