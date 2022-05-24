using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BlazorServerSamples.Web.Domain;
using Microsoft.Extensions.Logging;
using BlazorServerSamples.Web.SmartEnums;
using Blazored.FluentValidation;

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

	public List<WeeklyVideoTable>? WeeklyVideoTableList { get; set; }

	public List<YouTubeFeedModel>? YouTubeList { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Index) + "!" + nameof(OnInitializedAsync)));
		YouTubeList = await Svc.GetModel(SocialMedia.YouTube.YouTubeFeed(), 5);
		await PopulateWeeklyVideoTableList();
		PopulateWeeklyVideoAddVMList();
	}

	private void PopulateWeeklyVideoAddVMList()
	{
		Logger!.LogDebug(string.Format("...PopulateWeeklyVideoAddVMList Start"));

		foreach (var item in YouTubeList)
		{
			WeeklyVideoTable wvt = WeeklyVideoTableList.Find(x => x.YouTubeId == item.YouTubeId);

			if (wvt is null)
			{
				WeeklyVideoAddVMList.Add(new WeeklyVideoAddVM()
				{
					Title = item.Title,
					YouTubeId = item.YouTubeId,
				});
			}
		}
		Logger!.LogDebug(string.Format("...PopulateWeeklyVideoAddVMList End"));
	}


	private async Task PopulateWeeklyVideoTableList()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Index) + "!" + nameof(PopulateWeeklyVideoTableList)));
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
			Logger!.LogError(ex, $"...{DatabaseErrorMsg}");
		}
	}

	protected async Task WeeklyVideoInsertedCallback(int newId)
	{
		Logger!.LogDebug(string.Format("Inside {0}; newId: {1}"
			, nameof(Index) + "!" + nameof(WeeklyVideoInsertedCallback), newId));

		DatabaseInformation = true;
		DatabaseInformationMsg = $"Record Added; newId: {newId}";
		await PopulateWeeklyVideoTableList();
		WeeklyVideoAddVMList = new List<WeeklyVideoAddVM>();
		PopulateWeeklyVideoAddVMList();
		//StateHasChanged();
	}

	#region Events


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
	protected string DatabaseInformationMsg { get; set; } = String.Empty;
	protected bool DatabaseWarning = false;
	protected string DatabaseWarningMsg { get; set; } = String.Empty;
	protected bool DatabaseError { get; set; } // = false; handled by InitializeErrorHandling
	protected string DatabaseErrorMsg { get; set; } = String.Empty;


	#endregion

}
