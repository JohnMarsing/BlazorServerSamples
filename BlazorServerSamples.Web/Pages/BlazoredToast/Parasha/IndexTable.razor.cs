using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorServerSamples.Domain;
using System;
using Microsoft.Extensions.Logging;
using BlazorServerSamples.Data;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class IndexTable
{
	[Inject] 
	public ILogger<IndexTable> Logger { get; set; }
	
	[Inject]
	private IShabbatWeekRepository db { get; set; }
	
	protected IReadOnlyList<ParashaList> ParashaList;
	protected BibleBook BibleBook;
	protected Tuple<BibleBook, List<ParashaList>> ParashaListTuple;

	[Parameter]
	public bool IsXsOrSm { get; set; }

	[Parameter]
	public int BookId { get; set; } = 0;

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
			if (ParashaListTuple is not null)
			{
				Logger.LogDebug(string.Format("... Data gotten from DATABASE"));
				BibleBook = ParashaListTuple.Item1;
				ParashaList = ParashaListTuple.Item2;
			}
			else
			{
				DatabaseWarning = true;
				DatabaseWarningMsg = $"{nameof(ParashaListTuple)} NOT FOUND";
			}
		}
		catch (Exception ex)
		{
			DatabaseError = true;
			DatabaseErrorMsg = $"Error reading database";
			Logger.LogError(ex, string.Format("...Exception, DatabaseErrorMsg: {0}", DatabaseErrorMsg));
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

	#region ErrorHandling
	protected bool DatabaseError { get; set; } = false;
	protected string DatabaseErrorMsg { get; set; } = String.Empty;
	protected bool DatabaseWarning = false;
	protected string DatabaseWarningMsg { get; set; } = String.Empty;
	#endregion

}
