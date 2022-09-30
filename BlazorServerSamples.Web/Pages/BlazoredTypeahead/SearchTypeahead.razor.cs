using BlazorServerSamples.Web.Enums;
using Microsoft.AspNetCore.Components;

namespace BlazorServerSamples.Web.Pages.BlazoredTypeahead;

public partial class SearchTypeahead
{
	[Parameter] protected IEnumerable<BibleBook> BibleBooks { get; set; }

	//public VM VM { get; set; } = new VM();
	private FormExample FormModel = new FormExample();

	/*
	protected override async Task OnInitializedAsync()
	{
		await Task.Delay(0);
		VM = new VM();
	}
	*/

	private async Task<IEnumerable<BibleBook>> SearchBibleBooks(string searchText)
	{
		return await Task.FromResult(BibleBook.List
			.Where(x => x.Title.ToLower().Contains(searchText.ToLower()))
			.OrderBy(o => o.Value));
	}

	void HandlValidSubmit()
	{
		Console.WriteLine("Form Submitted Successfully!");
	}
	//protected BibleBook? SelectedBook;

}

