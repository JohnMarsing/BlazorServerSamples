using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Blazored.Toast.Services;
using BlazorServerSamples.Web.Pages.BlazoredToast.Parasha.Data;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class ParashotTable
{
	[Inject] 
	public ILogger<ParashotTable>? Logger { get; set; }
	
	[Inject]
	private IParashaRepository db { get; set; }
	
	[Inject]
	public IToastService toast { get; set; }

	protected IReadOnlyList<Parashot> Parashot;

	[Parameter]
	public bool IsXsOrSm { get; set; }

	[Parameter]
	public int BookId { get; set; } = 0;

	protected bool TurnSpinnerOff = false;

	protected string Colspan;
	protected int prevGregorianYear = 0;

	protected override async Task OnInitializedAsync()
	{
		Logger!.LogDebug(string.Format("Inside: {0}, Class!Method: {1}; BookId{2}"
			, "BlazoredToast.Parasha", nameof(ParashotTable) + "!" + nameof(OnInitializedAsync), BookId));

		Colspan = (!IsXsOrSm) ? "8" : "6";
		try
		{
			Parashot = await db.GetParashotByBookId(BookId);
			Logger!.LogDebug(string.Format("...After calling {0}", nameof(db.GetParashotByBookId)));
			if (Parashot is not null)
			{
				Logger!.LogDebug(string.Format("... Data gotten from DATABASE"));
			}
			else
			{
				toast.ShowWarning($"{nameof(Parashot)} NOT FOUND");
				Logger!.LogWarning(string.Format("...Parasha NOT found"));
			}
		}
		catch (Exception ex)
		{
			toast.ShowError($"Error reading database; {nameof(db.GetParashotByBookId)}");
			Logger!.LogError(ex, string.Format("...Exception reading database"));
		}
		finally
		{
			TurnSpinnerOff = true;
		}
	}

	public static string CurrentReadDateTextFormat(DateTime readDate)
	{
		DateTime compareDate = DateTime.Today;
		if (readDate >= compareDate & readDate <= compareDate.AddDays(6))
		{
			return "text-danger";
			//<span class='bg-danger'>@Title</span>
		}
		else
		{
			return "";
		}
	}

	public static string MyHebrewBibleParashaUrl(int id, string url)
	{
		string url2 = !String.IsNullOrEmpty(url) ? url : "";
		return "https://myhebrewbible.com/BlazoredToast.Parasha/Triennial/LivingMessiah/" + id.ToString() + "?slug=" + url2;
	}

}
