using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Page = BlazorServerSamples.Web.Links.Parasha;
using Blazored.Toast.Services;
using BlazorServerSamples.Web.Pages.BlazoredToast.Parasha.Data;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class Index
{
	[Inject]
	private IParashaRepository db { get; set; }

	[Inject]
	public ILogger<Index> Logger { get; set; }

	[Inject]
	public IToastService toast { get; set; }

	protected CurrentParasha? CurrentParasha;

	protected bool TurnSpinnerOff = false;

	protected override async Task OnInitializedAsync()
	{
		Logger.LogDebug(string.Format("Inside Page: {0}, Class!Method: {1}", Page.Index, nameof(Index) + "!" + nameof(OnInitializedAsync)));

		try
		{
			CurrentParasha = await db.GetCurrentParasha();
			Logger.LogDebug(string.Format("...After calling {0}; CurrentParasha: {1}", nameof(db.GetCurrentParasha), CurrentParasha));

			if (CurrentParasha is not null)
			{
				//toast.ShowInfo("Parasha gotten from the Database");
				Logger.LogDebug(string.Format("...Parasha gotten from DATABASE, Parasha: {0}", CurrentParasha));
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
