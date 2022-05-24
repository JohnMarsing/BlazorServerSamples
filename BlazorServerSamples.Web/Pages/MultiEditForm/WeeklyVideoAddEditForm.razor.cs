namespace BlazorServerSamples.Web.Pages.MultiEditForm;

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BlazorServerSamples.Web.Domain;
using Microsoft.Extensions.Logging;
using BlazorServerSamples.Web.SmartEnums;
using Blazored.FluentValidation;

public partial class WeeklyVideoAddEditForm
{
	[Parameter]
	public WeeklyVideoAddVM vm { get; set; }

	[Parameter]
	public EventCallback<int> OnWeeklyVideoInsertedCallback { get; set; }

	[Inject]
	public IWeeklyVideosRepository? db { get; set; }

	[Inject]
	public ILogger<WeeklyVideoAddEditForm>? Logger { get; set; }

	private FluentValidationValidator? _fluentValidationValidator;
	
	public List<ShabbatWeek>? ShabbatWeekList { get; set; }


	protected override async Task OnInitializedAsync()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(WeeklyVideoAddEditForm) + "!" + nameof(OnInitializedAsync)));
		await PopulateShabbatWeek();
	}


	#region Shabbat Week Lookup
	private int WeekCount = 3;

	private async Task PopulateShabbatWeek()
	{
		Logger!.LogDebug(string.Format("Inside {0}; WeekCount:{1}", nameof(WeeklyVideoAddEditForm) + "!" + nameof(PopulateShabbatWeek), WeekCount));

		try
		{
			ShabbatWeekList = await db!.GetShabbatWeekList(WeekCount);

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
			Logger!.LogError(ex, $"...{DatabaseErrorMsg}");
		}
	}

	#endregion


	void ClickPartialValidate(string setName)
	{
		Logger!.LogDebug(string.Format("..ClickPartialValidate for RuleSet {0}; result: {1}",
			setName, _fluentValidationValidator?.Validate(options => options.IncludeRuleSets(setName))));
	}

	protected async Task SubmitValidForm()
	{
		Logger!.LogDebug(string.Format("...{0}", nameof(Index) + "!" + nameof(SubmitValidForm)));

		int newId = 0;
		WeeklyVideoInsert dto = new WeeklyVideoInsert();
		dto.ShabbatWeekId = (int)vm.ShabbatWeekId;
		dto.WeeklyVideoTypeId = (int)vm.WeeklyVideoTypeId;
		dto.YouTubeId = vm.YouTubeId;
		dto.Title = vm.Title;
		dto.Book = 0;
		dto.Chapter = 0;

		try
		{
			newId = await db!.WeeklyVideoAdd(dto);
			vm.Title = "";
			vm.YouTubeId = "";
		}
		catch (Exception ex)
		{
			DatabaseError = true;
			DatabaseErrorMsg = $"Error inserting row in database";
			Logger!.LogError(ex, $"...{DatabaseErrorMsg}");
		}

		Logger!.LogDebug(string.Format("...newId: {0}", newId));
		await OnWeeklyVideoInsertedCallback.InvokeAsync(newId); // Call back
	}

	#region ErrorHandling
	private void InitializeErrorHandling()
	{
		DatabaseWarningMsg = "";
		DatabaseWarning = false;
		DatabaseErrorMsg = "";
		DatabaseError = false;
	}

	protected bool DatabaseWarning = false;
	protected string DatabaseWarningMsg { get; set; } = String.Empty;
	protected bool DatabaseError { get; set; } 
	protected string DatabaseErrorMsg { get; set; } = String.Empty;

	#endregion


}
