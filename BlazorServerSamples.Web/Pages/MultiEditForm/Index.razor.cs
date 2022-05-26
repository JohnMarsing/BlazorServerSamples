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

	public List<AddEditFormVM> AddEditFormVMList { get; set; } = new List<AddEditFormVM>();

	public List<WeeklyVideoTable>? VideosPreviouslyAdded { get; set; }

	public List<YouTubeFeedModel>? YouTubeList { get; set; }

	public bool ShahbatWeekSelected { get; set; } = false;
	public int SelectedShabbatWeekId { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Index) + "!" + nameof(OnInitializedAsync)));
		YouTubeList = await Svc.GetModel(SocialMedia.YouTube.YouTubeFeed(), 5);
	}

	protected async Task ShabbatWeekIdChosenCallback(int id)
	{
		Logger!.LogDebug(string.Format("Inside {0}; shabbatWeek: {1}"
			, nameof(Index) + "!" + nameof(ShabbatWeekIdChosenCallback), id));
		ShahbatWeekSelected = true;
		SelectedShabbatWeekId = id;
		await PopulateVideosPreviouslyAdded();
		PopulateAddEditFormVMList();
	}

	private async Task PopulateVideosPreviouslyAdded()
	{
		Logger!.LogDebug(string.Format("Inside {0}, SelectedShabbatWeekId:{1}"
			, nameof(Index) + "!" + nameof(PopulateVideosPreviouslyAdded), SelectedShabbatWeekId));
		try
		{
			VideosPreviouslyAdded = await db!.GetWeeklyVideoByShabbatWeekId(SelectedShabbatWeekId);

			if (VideosPreviouslyAdded is null)
			{
				DatabaseWarning = true;
				DatabaseWarningMsg = $"{nameof(VideosPreviouslyAdded)} NOT FOUND";
			}
		}
		catch (Exception ex)
		{
			DatabaseError = true;
			DatabaseErrorMsg = $"Error reading database (WeeklyVideoTableList)";
			Logger!.LogError(ex, $"...{DatabaseErrorMsg}");
		}
	}

	private void PopulateAddEditFormVMList()
	{
		Logger!.LogDebug(string.Format("...PopulateWeeklyVideoAddVMList"));
		AddEditFormVMList = new List<AddEditFormVM>();

		if (YouTubeList is not null && VideosPreviouslyAdded is not null)
		{
			foreach (var item in YouTubeList)
			{
				WeeklyVideoTable wvt = VideosPreviouslyAdded!.Find(x => x.YouTubeId == item.YouTubeId);

				if (wvt is null)
				{
					AddEditFormVMList.Add(new AddEditFormVM()
					{
						Title = item.Title,
						YouTubeId = item.YouTubeId,
						ShabbatWeekId = SelectedShabbatWeekId
					}); ;
				}
			}
		}
		
	}

	protected async Task WeeklyVideoInsertedCallback(int newId)
	{
		Logger!.LogDebug(string.Format("Inside {0}; newId: {1}"
			, nameof(Index) + "!" + nameof(WeeklyVideoInsertedCallback), newId));

		DatabaseInformation = true;
		DatabaseInformationMsg = $"Record Added; newId: {newId}";
		await PopulateVideosPreviouslyAdded();
		PopulateAddEditFormVMList();
	}

	protected async Task DeleteClick(int id)
	{
		Logger!.LogDebug(string.Format("...{0}; id:{1}"
			, nameof(DeleteClick), id));
		int affectedRows = await db!.WeeklyVideoDelete(id);
		Logger!.LogDebug(string.Format("...affectedRows: {0}", affectedRows));
		await PopulateVideosPreviouslyAdded();
		PopulateAddEditFormVMList();
	}


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
