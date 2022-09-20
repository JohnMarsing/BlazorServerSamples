using Microsoft.AspNetCore.Components;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha;

public partial class ParashotTableToggle
{
	[Parameter]
	public string CardCss { get; set; } = "border-primary my-3";

	[Parameter]
	public string HeaderBadgeColor { get; set; } = "bg-warning";

	[Parameter]
	public string Title { get; set; } = "Parasha Table";

	[Parameter]
	public bool IsXsOrSm { get; set; } = false;

	[Parameter]
	public int BookId { get; set; } = 0;

	public string ButtonText { get; set; } = "Details";
	public string ButtonChevron { get; set; } = " bi bi-chevron-down"; //  " fas fa-chevron-down";

	public bool IsCollapsed { get; set; } = true;

	protected void ToggleButtonClick(bool isCollapsed)
	{
		IsCollapsed = !isCollapsed;
		ButtonText = IsCollapsed ? "Details" : "Hide";
		ButtonChevron = IsCollapsed ? "bi bi-chevron-down" : "bi bi-chevron-up"; //  "fas fa-chevron-down" : "fas fa-chevron-up"
	}
}
