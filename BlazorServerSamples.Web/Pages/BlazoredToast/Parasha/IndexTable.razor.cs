using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorServerSamples.Domain;
using System;
using Microsoft.Extensions.Logging;
using BlazorServerSamples.Data;
using Blazored.Toast.Services;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class IndexTable
{
	[Inject] 
	public ILogger<IndexTable> Logger { get; set; }
	
	[Inject]
	private IShabbatWeekRepository db { get; set; }
	
	[Inject]
	public IToastService toast { get; set; }

	protected IReadOnlyList<ParashaList> ParashaList;
	protected BibleBook BibleBook;
	protected Tuple<BibleBook, List<ParashaList>> ParashaListTuple;

	[Parameter]
	public bool IsXsOrSm { get; set; }

	[Parameter]
	public int BookId { get; set; } = 0;

	protected bool TurnSpinnerOff = false;

	protected string Colspan;
	protected int prevGregorianYear = 0;

	protected override async Task OnInitializedAsync()
	{
		Logger.LogDebug(string.Format("Inside: {0}, Class!Method: {1}; BookId{2}"
			, "BlazoredToast.Parasha", nameof(IndexTable) + "!" + nameof(OnInitializedAsync), BookId));

		Colspan = (!IsXsOrSm) ? "8" : "6";
		try
		{
			ParashaListTuple = await db.GetParashotByBookId(BookId);
			Logger.LogDebug(string.Format("...After calling {0}", nameof(db.GetParashotByBookId)));
			if (ParashaListTuple is not null)
			{
				Logger.LogDebug(string.Format("... Data gotten from DATABASE"));
				//toast.ShowInfo("Data gotten from the Database");
				BibleBook = ParashaListTuple.Item1;
				ParashaList = ParashaListTuple.Item2;
			}
			else
			{
				toast.ShowWarning($"{nameof(ParashaListTuple)} NOT FOUND");
				Logger.LogWarning(string.Format("...Parasha NOT found"));
			}
		}
		catch (Exception ex)
		{
			toast.ShowError($"Error reading database; {nameof(db.GetParashotByBookId)}");
			Logger.LogError(ex, string.Format("...Exception reading database"));
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
