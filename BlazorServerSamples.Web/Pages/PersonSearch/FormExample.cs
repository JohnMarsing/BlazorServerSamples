using System.ComponentModel.DataAnnotations;

namespace BlazorServerSamples.Web.Pages.PersonSearch;

public class FormExample
{
	[Required]
	public Person SelectedPerson { get; set; }
}

