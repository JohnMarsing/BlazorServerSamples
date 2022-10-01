using System.ComponentModel.DataAnnotations;

namespace BlazorServerSamples.Web.Pages.PersonSearch;

public class FormValidation
{
	[Required]
	[Display(Name = "Person")]
	public Person SelectedPerson { get; set; }
}

