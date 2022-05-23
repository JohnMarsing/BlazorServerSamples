
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BlazorServerSamples.Web.Domain;
using Microsoft.Extensions.Logging;
using BlazorServerSamples.Web.SmartEnums;
using Blazored.FluentValidation;


namespace BlazorServerSamples.Web.Pages.MultiEditForm;

public partial class SingleEditForm
{
	[Inject]
	public IYouTubeFeedService? Svc { get; set; }

	[Inject]
	public IWeeklyVideosRepository? db { get; set; }

	[Inject]
	public ILogger<SingleEditForm>? Logger { get; set; }

	public WeeklyVideoAddVM WeeklyVideoAddVM { get; set; } = new WeeklyVideoAddVM();

	public List<ShabbatWeek>? ShabbatWeekList { get; set; }
	public List<YouTubeFeedModel>? YouTubeList { get; set; }
	public List<WeeklyVideoTable>? WeeklyVideoTableList { get; set; }

	private FluentValidationValidator? _fluentValidationValidator;

	void ClickPartialValidate(string setName)
	{
		Logger!.LogDebug(string.Format("ClickPartialValidate for RuleSet {0}; result: {1}",
			setName, _fluentValidationValidator?.Validate(options => options.IncludeRuleSets(setName))));
	}

	public bool IsLoading { get; set; } = true;
	public bool InsufficientData { get; set; } = true;

	#region Shabbat Week Lookup
	private int WeekCount = 3;

	private async Task PopulateShabbatWeek()
	{
		Logger.LogDebug(string.Format("Inside {0}; WeekCount:{1}", nameof(SingleEditForm) + "!" + nameof(PopulateShabbatWeek), WeekCount));

		try
		{
			ShabbatWeekList = await db.GetShabbatWeekList(WeekCount);

			if (ShabbatWeekList is null)
			{
				DatabaseWarning = true;
				DatabaseWarningMsg = $"{nameof(ShabbatWeekList)} NOT FOUND";
			}

		}
		catch (Exception ex)
		{
			DatabaseError = true;
			DatabaseErrorMsg = $"Error reading database";
			Logger.LogError(ex, $"...{DatabaseErrorMsg}");
		}
	}

	#endregion

	protected override async Task OnInitializedAsync()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(SingleEditForm) + "!" + nameof(OnInitializedAsync)));
		await PopulateShabbatWeek();
		YouTubeList = await Svc.GetModel(SocialMedia.YouTube.YouTubeFeed(), 5);
		await PopulateWeeklyVideoTableList();
		IsLoading = false;
	}

	private async Task PopulateWeeklyVideoTableList()
	{
		Logger!.LogDebug(string.Format("Inside {0}; WeekCount:{1}", nameof(SingleEditForm) + "!" + nameof(PopulateWeeklyVideoTableList), WeekCount));
		try
		{
			WeeklyVideoTableList = await db.GetWeeklyVideoTableList(5);

			if (WeeklyVideoTableList is null)
			{
				DatabaseWarning = true;
				DatabaseWarningMsg = $"{nameof(WeeklyVideoTableList)} NOT FOUND";
			}
			else
			{
				if (WeeklyVideoTableList.Count == 0)
				{
					DatabaseWarning = true;
					DatabaseWarningMsg = $"{nameof(WeeklyVideoTableList)} Count is 0";
				}
			}
		}
		catch (Exception ex)
		{
			DatabaseError = true;
			DatabaseErrorMsg = $"Error reading database (WeeklyVideoTableList)";
			Logger.LogError(ex, $"...{DatabaseErrorMsg}");
		}
	}
	#region Events

	protected async Task HandleAddClick()
	{
		Logger.LogDebug(string.Format("...{0}", nameof(SingleEditForm) + "!" + nameof(HandleAddClick)));
		DatabaseInformation = false;
		DatabaseInformationMsg = "";

		int newId = 0;
		WeeklyVideoInsert dto = new WeeklyVideoInsert();
		dto.ShabbatWeekId = WeeklyVideoAddVM.ShabbatWeekId;
		dto.WeeklyVideoTypeId = WeeklyVideoAddVM.WeeklyVideoTypeId;
		dto.YouTubeId = WeeklyVideoAddVM.YouTubeId;
		dto.Title = WeeklyVideoAddVM.Title;
		dto.Book = 0;
		dto.Chapter = 0;

		try
		{
			newId = await db.WeeklyVideoAdd(dto);
			WeeklyVideoAddVM.Title = "";
			WeeklyVideoAddVM.YouTubeId = "";
		}
		catch (Exception ex)
		{
			DatabaseError = true;
			DatabaseErrorMsg = $"Error inserting row in database";
			Logger.LogError(ex, $"...{DatabaseErrorMsg}");
		}

		Logger.LogDebug(string.Format("...newId: {0}", newId));

		DatabaseInformation = true;
		DatabaseInformationMsg = $"Record Added; newId: {newId}";

		await PopulateWeeklyVideoTableList();
		StateHasChanged(); // let's try this here
	}

	#endregion


	#region ErrorHandling
	private void InitializeErrorHandling()
	{
		DatabaseInformationMsg = "";
		DatabaseInformation = false;
		DatabaseWarningMsg = "";
		DatabaseWarning = false;
		DatabaseErrorMsg = "";
		DatabaseError = false;
	}

	protected bool DatabaseInformation = false;
	protected string DatabaseInformationMsg { get; set; }
	protected bool DatabaseWarning = false;
	protected string DatabaseWarningMsg { get; set; }
	protected bool DatabaseError { get; set; } // = false; handled by InitializeErrorHandling
	protected string DatabaseErrorMsg { get; set; }


	#endregion

}
