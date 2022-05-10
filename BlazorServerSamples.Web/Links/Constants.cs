namespace BlazorServerSamples.Web.Links;

public static class Home
{
	public const string Index = "/";
	public const string Title = "Blazor Server Samples Home Page";
	public const string PageTitle = "Home | BSS";
	public const string Icon = "fas fa-home";
}

public static class About
{
	public const string Index = "/About";
	public const string Title = "About";
	public const string Icon = "fas fa-info";
}

public static class Sitemap
{
	public const string Index = "/Sitemap";
	public const string Title = "Sitemap";
	public const string Icon = "fas fa-sitemap";
}

public static class DashBoard
{
	public const string Index = "/Admin/Dashboard/Index";
	public const string Title = "DashBoard";
	public const string Icon = "fas fa-tachometer-alt";
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
