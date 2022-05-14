namespace BlazorServerSamples.Web.Pages.MultiEditForm;

public class YouTubeFeedModel
{
	public int? Id { get; set; }
	public string YouTubeId { get; set; } = String.Empty;
	public string Title { get; set; } = String.Empty;
	public DateTimeOffset PublishDate { get; set; }
}
