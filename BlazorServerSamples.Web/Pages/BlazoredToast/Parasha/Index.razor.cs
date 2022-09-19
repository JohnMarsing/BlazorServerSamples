using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Page = BlazorServerSamples.Web.Links.Parasha;
using BlazorServerSamples.Data;
using Blazored.Toast.Services;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class Index
{
	[Inject]
	private IShabbatWeekRepository db { get; set; }

	[Inject]
	public ILogger<Index> Logger { get; set; }

	[Inject]
	public IToastService toast { get; set; }

	protected BlazorServerSamples.Domain.Parasha Parasha;

	protected bool TurnSpinnerOff = false;

	protected override async Task OnInitializedAsync()
	{
		Logger.LogDebug(string.Format("Inside Page: {0}, Class!Method: {1}", Page.Index, nameof(Index) + "!" + nameof(OnInitializedAsync)));

		try
		{
			Parasha = await db.GetCurrentParashaAndChildren();
			Logger.LogDebug(string.Format("...After calling {0}; Parasha: {1}", nameof(db.GetCurrentParashaAndChildren), Parasha));

			if (Parasha is not null)
			{
				//toast.ShowInfo("Parasha gotten from the Database");
				Logger.LogDebug(string.Format("...Parasha gotten from DATABASE, Parasha: {0}", Parasha));
			}
			else
			{
				toast.ShowWarning($"Could not load because Current Parasha Unknown");
				Logger.LogWarning(string.Format("...Parasha NOT found"));
			}

		}
		catch (Exception ex)
		{
			toast.ShowError($"Error reading database");
			Logger.LogError(ex, string.Format("...Exception reading database"));
		}
		finally
		{
			TurnSpinnerOff = true;
		}

	}

}
