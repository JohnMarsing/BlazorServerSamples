using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BlazorServerSamples.Web.Domain;
using Microsoft.Extensions.Logging;
using BlazorServerSamples.Web.SmartEnums;

namespace BlazorServerSamples.Web.Pages.MultiEditForm;

public partial class Index
{
	[Inject]
	public IYouTubeFeedService? Svc { get; set; }

	[Inject]
	public IWeeklyVideosRepository? db { get; set; }

	[Inject]
	public ILogger<Index>? Logger { get; set; }
	
	public List<WeeklyVideoAddVM> WeeklyVideoAddVMList { get; set; } = new List<WeeklyVideoAddVM>();

	public List<ShabbatWeek>? ShabbatWeekList { get; set; }
	public List<YouTubeFeedModel>? YouTubeList { get; set; }
	public List<WeeklyVideoTable>? WeeklyVideoTableList { get; set; }

	public bool IsLoading { get; set; } = true;
	public bool InsufficientData { get; set; } = true;

	#region Shabbat Week Lookup
	private int WeekCount = 3;

	private async Task PopulateShabbatWeek()
	{
		Logger.LogDebug(string.Format("Inside {0}; WeekCount:{1}", nameof(Index) + "!" + nameof(PopulateShabbatWeek), WeekCount));

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
		Logger.LogDebug(string.Format("Inside {0}", nameof(Index) + "!" + nameof(OnInitialized)));
		await PopulateShabbatWeek();
		YouTubeList = await Svc.GetModel(SocialMedia.YouTube.YouTubeFeed(), 5);
		await PopulateWeeklyVideoTableList();
		PopulateWeeklyVideoAddVMList();
		IsLoading = false;
	}

	private int _shabbatWeekId = 1;
	private void PopulateWeeklyVideoAddVMList()
	{
		Logger.LogDebug(string.Format("...PopulateWeeklyVideoAddVMList Start"));

		foreach (var item in YouTubeList)
		{
			WeeklyVideoTable wvt = WeeklyVideoTableList.Find(x => x.YouTubeId == item.YouTubeId);

			if (wvt is null)
			{
				WeeklyVideoAddVMList.Add(new WeeklyVideoAddVM()
				{
					WeeklyVideoTypeId = WeeklyVideoType.MainServiceEnglish,
					ShabbatWeekId = _shabbatWeekId,
					Title = item.Title,
					YouTubeId = item.YouTubeId,
				});
			}
		}
		Logger.LogDebug(string.Format("...PopulateWeeklyVideoAddVMList End"));
	}


	private async Task PopulateWeeklyVideoTableList()
	{
		Logger.LogDebug(string.Format("Inside {0}; WeekCount:{1}", nameof(Index) + "!" + nameof(PopulateWeeklyVideoTableList), WeekCount));
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

	protected async Task HandleAddClick(WeeklyVideoAddVM weeklyVideoAddVM)
	{
		Logger.LogDebug(string.Format("...{0}", nameof(Index) + "!" + nameof(HandleAddClick)));
		DatabaseInformation = false;
		DatabaseInformationMsg = "";

		int newId = 0;
		WeeklyVideoInsert dto = new WeeklyVideoInsert();
		dto.ShabbatWeekId = weeklyVideoAddVM.ShabbatWeekId;
		dto.WeeklyVideoTypeId = weeklyVideoAddVM.WeeklyVideoTypeId;
		dto.YouTubeId = weeklyVideoAddVM.YouTubeId;
		dto.Title = weeklyVideoAddVM.Title;
		dto.Book = 0;
		dto.Chapter = 0;

		try
		{
			newId = await db.WeeklyVideoAdd(dto);
			weeklyVideoAddVM.Title = "";
			weeklyVideoAddVM.YouTubeId = "";
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
