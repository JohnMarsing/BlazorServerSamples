using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Blazored.Toast.Services;
using BlazorServerSamples.Web.Pages.BlazoredToast.Parasha.Services;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class ParashotTable
{
	[Inject]
	private IParashaService Service { get; set; }

	[Inject] 
	public ILogger<ParashotTable>? Logger { get; set; }
	
	[Inject]
	public IToastService Toast { get; set; }

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
		Logger!.LogDebug(string.Format("Inside: {0}, Class!Method: {1}; BookId: {2}"
			, "BlazoredToast.Parasha", nameof(ParashotTable) + "!" + nameof(OnInitializedAsync), BookId));

		Colspan = (!IsXsOrSm) ? "8" : "6";

		try
		{
			Parashot = await Service.GetParashotByBookId(BookId);
			//Logger!.LogDebug(string.Format("...just called service.{0}; BookId: {1}"
			//	, nameof(Service.GetParashotByBookId), BookId));

			if (Parashot is null || !Parashot.Any())
			{
				Toast.ShowWarning(Service.UserInterfaceMessage);
			}

		}
		//catch (ParashaService.ParashotListNotFoundException parashotListNotFoundException)
		//{
		//	Toast.ShowWarning(parashotListNotFoundException.Message);
		//}

		catch (InvalidOperationException invalidOperationException)
		{
			Toast.ShowError(invalidOperationException.Message);
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
		return "https://myhebrewbible.com/Parasha/Triennial/LivingMessiah/" + id.ToString() + "?slug=" + url2;
	}

}
