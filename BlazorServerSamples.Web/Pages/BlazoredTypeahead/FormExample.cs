using System.ComponentModel.DataAnnotations;
namespace BlazorServerSamples.Web.Pages.BlazoredTypeahead;


public class FormExample
{
	[Required]
	public Chapter SelectedChapter { get; set; }
}

/*
public class VM
{
	public string? Title { get; set; }
	public string? Abrv { get; set; }
	public BookGroupEnum BookGroupEnum { get; set; }
	public BookEnum BookEnum { get; set; }
	public int LastChapter { get; set; }
	public string? TransliterationInHebrew { get; set; }
	public string? NameInHebrew { get; set; }
}
*/