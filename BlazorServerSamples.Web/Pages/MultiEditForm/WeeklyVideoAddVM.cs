namespace BlazorServerSamples.Web.Pages.MultiEditForm;
using System.ComponentModel.DataAnnotations;

public class WeeklyVideoAddVM
{
	[Required]
	public int WeeklyVideoTypeId { get; set; }

	[Required]
	public int ShabbatWeekId { get; set; }

	[Required]
	[StringLength(11, MinimumLength = 3, ErrorMessage = "length {0} must be between {2} and {1}.")]
	public string YouTubeId { get; set; } = String.Empty;

	[Required]
	public string Title { get; set; } = String.Empty;
}
