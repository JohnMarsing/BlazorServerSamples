using System.ComponentModel.DataAnnotations;
using BlazorServerSamples.Web.Enums;

namespace BlazorServerSamples.Web.Pages.BibleSearch;

public class FormValidation
{
	[Required]
	public BibleBook SelectedBook { get; set; }

	public string MhbUrl
	{
		get => "https://myhebrewbible.com/BookChapter/" + SelectedBook.Title + "/" + SelectedBook.Value + "/slug";
	}

	public string Mhb(int chapter)
	{
		return "https://myhebrewbible.com/BookChapter/" + SelectedBook.Title + "/" + chapter + "/slug";
	}

}

