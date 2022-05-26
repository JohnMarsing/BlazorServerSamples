namespace BlazorServerSamples.Web.Pages.MultiEditForm;

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BlazorServerSamples.Web.Domain;
using Microsoft.Extensions.Logging;
using BlazorServerSamples.Web.SmartEnums;
using Blazored.FluentValidation;

public partial class ShabbatWeekChooser
{
	[Parameter]
	public EventCallback<int> OnShabbatWeekIdChosenCallback { get; set; }

	[Inject]
	public ILogger<ShabbatWeekChooser>? Logger { get; set; }

	[Inject]
	public IWeeklyVideosRepository? db { get; set; }

	public List<ShabbatWeek>? ShabbatWeekList { get; set; }

	public int WeekCount { get; set; } = 3;

	protected override async Task OnInitializedAsync()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(ShabbatWeekChooser) + "!" + nameof(OnInitializedAsync)));
		await PopulateShabbatWeek();
	}

	private async Task PopulateShabbatWeek()
	{
		Logger!.LogDebug(string.Format("Inside {0}; WeekCount:{1}"
			, nameof(ShabbatWeekChooser) + "!" + nameof(PopulateShabbatWeek), WeekCount));

		try
		{
			ShabbatWeekList = await db!.GetShabbatWeekList(WeekCount);

			if (ShabbatWeekList is null)
			{
				Console.WriteLine("(ShabbatWeekList is null");
			}

		}
		catch (Exception ex)
		{
			Console.WriteLine("Error reading database");
			Logger!.LogError(ex, $"...Error reading database");
		}

	}

	protected async Task OnShabbatWeekIdChanged(ChangeEventArgs e)
	{
		Logger!.LogDebug(string.Format("...{0}", nameof(OnShabbatWeekIdChanged)));
		int selectedId = int.TryParse(e.Value.ToString(), out selectedId) ? selectedId : 0;
		Logger!.LogDebug(string.Format("...Selected Id: {0}", selectedId));
		await OnShabbatWeekIdChosenCallback.InvokeAsync(selectedId);

	}

	protected async Task OnWeekCountChanged(ChangeEventArgs e)
	{
		Logger!.LogDebug(string.Format("...{0}", nameof(OnWeekCountChanged)));
		int weekCount = int.TryParse(e.Value.ToString(), out weekCount) ? weekCount : 0;
		Logger!.LogDebug(string.Format("...Selected Id: {0}", weekCount));
		WeekCount = weekCount;
		await PopulateShabbatWeek();
	}


	protected async void  Requery(ChangeEventArgs e)
	{
		Logger!.LogDebug(string.Format("...{0}", nameof(Requery)));
		int weekCount = int.TryParse(e.Value.ToString(), out weekCount) ? weekCount : 0;
		Logger!.LogDebug(string.Format("...weekCount: {0}", weekCount));
		WeekCount = weekCount;
		//PopulateShabbatWeek();
		await PopulateShabbatWeek();
		//return BgEnumChanged.InvokeAsync(book.BookGroupEnum);
	}

	void ClickTest()
	{
		Logger!.LogDebug(string.Format("...Clicked Test"));
	}

}

/*

public ShabbatWeekChooserVM vm { get; set; }

public class ShabbatWeekChooserVM
{
	public int? ShabbatWeekId { get; set; }
	public int WeekCount { get; set; } = 3;
}
*/

