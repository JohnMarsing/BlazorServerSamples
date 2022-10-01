using BlazorServerSamples.Web.Enums;

namespace BlazorServerSamples.Web.Pages.BibleSearch;

public partial class Index
{
	private FormValidation VM = new FormValidation();

	private List<Person> People = new List<Person>();

	private async Task<IEnumerable<BibleBook>> SearchBibleBooks(string searchText)
	{
		return await Task.FromResult(BibleBook.List
			.Where(x => x.Title.ToLower().Contains(searchText.ToLower()))
			.OrderBy(o => o.Value));
	}

	private void HandleFormSubmit()  // Used only by Form
	{
		Console.WriteLine("Form Submitted Successfully!");
	}

}
