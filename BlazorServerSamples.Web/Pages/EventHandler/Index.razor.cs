namespace BlazorServerSamples.Web.Pages.EventHandler;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;


public partial class Index
{
	private string currentHeading = "Initial heading";
	private string newHeading = string.Empty;
	private string checkedMessage = "Not changed yet";

	private async Task UpdateHeading()
	{
		await Task.Delay(2000);
		currentHeading = $"{newHeading}!!!";
	}

	private void CheckChanged()
	{
		checkedMessage = $"Last changed at {DateTime.Now}";
	}

	private string heading2 = "Select a button to learn its position";
	private void UpdateHeading(MouseEventArgs e, int buttonNumber)
	{
		heading2 = $"Selected #{buttonNumber} at {e.ClientX}:{e.ClientY}";
	}

}
