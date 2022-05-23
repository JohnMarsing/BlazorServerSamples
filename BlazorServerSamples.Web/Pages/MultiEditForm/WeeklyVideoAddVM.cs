namespace BlazorServerSamples.Web.Pages.MultiEditForm;

using FluentValidation;


public class WeeklyVideoAddVM
{
	public int WeeklyVideoTypeId { get; set; }
	public int ShabbatWeekId { get; set; }
	public string YouTubeId { get; set; } = String.Empty;
	public string Title { get; set; } = String.Empty;
	//public BookChapter BookChapter { get; set; } = new();

	/*
	Logger.LogDebug(string.Format("...weeklyVideoAddVM: {0}", weeklyVideoAddVM.ToString() ));

	public override string ToString()
	{
		return $"Dump: {{ VT Id: {WeeklyVideoTypeId}; SH Id: {ShabbatWeekId}; YT Id: {YouTubeId}; Title: {Title} }}";
	}
	*/
}

public class WeeklyVideoAddVMValidator : AbstractValidator<WeeklyVideoAddVM>
{
	public WeeklyVideoAddVMValidator()
	{
		RuleSet("Select", () =>
		{
			RuleFor(w => w.ShabbatWeekId)
				.NotEmpty()
				.WithMessage("Shabbat Week can not be null");
			RuleFor(w => w.WeeklyVideoTypeId)
				.NotEmpty()
				.WithMessage("You must select a Weekly Video Type");
			//RuleFor(w => w.ShabbatWeekId)
			//	.NotEmpty().WithMessage("You must select a ShabbatWeekId");
		});

		RuleSet("TitleOnly", () =>
		{
			RuleFor(w => w.Title).MaximumLength(100).WithMessage("Title cannot be longer than 100 characters");
			RuleFor(w => w.Title).NotEmpty().WithMessage("Title required");
		});

		/*
		 
			YouTubeId is a hidden field so I don't think I should have this rule...not sure
				RuleFor(w => w.YouTubeId).Length(11).WithMessage("Must be 11 characters");
				RuleFor(w => w.YouTubeId).NotEmpty().WithMessage("You must enter a YouTube Id");
		 */
	}
}
