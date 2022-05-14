namespace BlazorServerSamples.Web.Pages.MultiEditForm;

public class WeeklyVideoInsert
{
	public int WeeklyVideoTypeId { get; set; }
	public int ShabbatWeekId { get; set; }
	public string YouTubeId { get; set; } = String.Empty;
	public string Title { get; set; } = String.Empty;
	public int Book { get; set; }
	public int Chapter { get; set; }
}
