namespace BlazorServerSamples.Web.Links;

/*
https://icons.getbootstrap.com/
*/

public static class Home
{
	public const string Index = "/";
	public const string Title = "Blazor Server Samples Home Page";
	public const string PageTitle = "Home | BSS";
	public const string Icon = "bi bi-house"; // fas fa-home
}

public static class About
{
	public const string Index = "/About";
	public const string Title = "About";
	public const string Icon = "bi bi-info-circle"; // "fas fa-info";
}

public static class BibleSearch
{
	public const string Index = "/BibleSearch";
	public const string Title = "Bible Search";
	public const string Icon = "bi bi-search";
}

public static class BlazedFvSingle
{
	public const string Index = "/BlazedFvSingle";
	public const string Title = "Blazed FluentValidation Person";
	public const string SubTitle = "Blazed FV Person";
	public const string Icon = "bi bi-bricks";
}

public static class BlazedFvMulti
{
	public const string Index = "/BlazedFvMulti";
	public const string Title = "Blazed FluentValidation People";
	public const string SubTitle = "Blazed FV People";
	public const string Icon = "bi bi-bricks";
}

public static class DashBoard
{
	public const string Index = "/Admin/Dashboard/Index";
	public const string Title = "DashBoard";
	public const string Icon = "bi bi-speedometer";  // fas fa-tachometer-alt
}

public static class CascadingValue
{
	public const string Index = "/CascadingValue";
	public const string Title = "Cascading Values";
	public const string Icon = "bi bi-filter-right";  
}

public static class NavigationSearch
{
	public const string Index = "/NavigationSearch";
	public const string Title = "Navigation Search";
	public const string Icon = "bi bi-search"; // "fas fa-torah";
}

public static class Parasha
{
	public const string Index = "/Parasha";
	public const string Title = "Parasha";
	public const string Icon = "bi bi-book"; // "fas fa-torah";
	public const string IconCurrent = "bi bi-bookmark"; // "far fa-bookmark";

	public const string IndexPrint = "/Parasha/IndexPrint";
	//public const string TitlePrint = "Parashot - Living Messiah"

	public static class MyHebrewBible
	{
		private const string baseUrl = "https://myhebrewbible.com/Parasha/Triennial/LivingMessiah/";
		public static string ParashaUrl(int id, string slug)
		{
			return $"{baseUrl}/{id}?slug={slug}/";
		}
	}
}

public static class People
{
	public const string Index = "/People";
	public const string Title = "Blazored Typeahead";
	public const string Icon = "bi bi-search"; 
}

public static class Sitemap
{
	public const string Index = "/Sitemap";
	public const string Title = "Sitemap";
	public const string Icon = "bi bi-diagram-3"; //"fas fa-sitemap";
}

public static class SmartEnums
{
	public const string Index = "/SmartEnums";
	public const string Title = "SmartEnums";
	public const string Icon = "bi bi-smartwatch";
}

public static class Unobtrusive
{
	public const string Index = "/Unobtrusive";
	public const string Title = "Unobtrusive";
	public const string SubTitle = "Example of unobtrusiveness in markup using Pretty Blazor";
	public const string Icon = "bi bi-emoji-smile";  // fas fa-tachometer-alt
}
// Nested example
public static class PayPal
{
	public static class Donate
	{
		public const string Index = "/donate";
		public const string Title = "Donate";
		public const string TitleLMM = "Donate Title";
		public const string Icon = "fab fa-paypal";
	}
	public static class CancelDonation
	{
		public const string Index = "/cancel_donation.html";
		public const string Title = "Donation Cancelation";
		public const string Icon = "fab fa-paypal";
	}
	public static class ConfirmDonation
	{
		public const string Index = "/confirm_donation.html";
		public const string Title = "Donation Confirmation";
		public const string Icon = "fab fa-paypal";
	}

}
