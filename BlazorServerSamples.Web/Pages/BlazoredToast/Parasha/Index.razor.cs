using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Page = BlazorServerSamples.Web.Links.Parasha;
using BlazorServerSamples.Data;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class Index
{
	[Inject]
	private IShabbatWeekRepository db { get; set; }

	[Inject]
	public ILogger<Index> Logger { get; set; }

	protected BlazorServerSamples.Domain.Parasha Parasha;

	protected override async Task OnInitializedAsync()
	{
		Logger.LogDebug(string.Format("Inside Page: {0}, Class!Method: {1}", Page.Index, nameof(Index) + "!" + nameof(OnInitializedAsync)));

		try
		{
			Logger.LogDebug(string.Format("...Key NOT found in cache, calling {0}", nameof(db.GetCurrentParashaAndChildren)));
			Parasha = await db.GetCurrentParashaAndChildren();
			Logger.LogDebug(string.Format("...After calling {0}; Parasha: {1}", nameof(db.GetCurrentParashaAndChildren), Parasha));

			if (Parasha is not null)
			{
				Logger.LogDebug(string.Format("...Parasha gotten from DATABASE, Parasha: {0}", Parasha));
			}
			else
			{
				DatabaseWarning = true;
				DatabaseWarningMsg = "Could not load because Current Parasha Unknown";
				Logger.LogDebug(string.Format("...Parasha NOT found, DatabaseWarningMsg: {0}", DatabaseWarningMsg));
			}

		}
		catch (Exception ex)
		{
			DatabaseError = true;
			DatabaseErrorMsg = $"Error reading database";
			Logger.LogError(ex, string.Format("...Exception, DatabaseErrorMsg: {0}", DatabaseErrorMsg));
		}



	}

	#region ErrorHandling
	protected bool DatabaseInformation = false;
	protected string DatabaseInformationMsg { get; set; } = string.Empty;
	protected bool DatabaseWarning = false;
	protected string DatabaseWarningMsg { get; set; } = string.Empty;
	protected bool DatabaseError { get; set; }
	protected string DatabaseErrorMsg { get; set; } = string.Empty;
	#endregion

}
