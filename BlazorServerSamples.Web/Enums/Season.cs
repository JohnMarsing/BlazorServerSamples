﻿using Ardalis.SmartEnum;

namespace BlazorServerSamples.Web.Enums;

public abstract class Season : SmartEnum<Season>
{
		#region Id's
		private static class Id
		{
				internal const int Fall = 1;
				internal const int Winter = 2;
				internal const int Spring = 3;
				internal const int Summer = 4;
				internal const int FallEOY = 5;
		}
		#endregion


		#region Declared Public Instances
		public static readonly Season Fall = new FallSE();
		public static readonly Season Winter = new WinterSE();
		public static readonly Season Spring = new SpringSE();
		public static readonly Season Summer = new SummerSE();
		public static readonly Season FallEOY = new FallEOYSE();
		#endregion

		private Season(string name, int value) : base(name, value) { } // Constructor

		#region Extra Fields
		public abstract string Type { get; }
		public abstract string BadgeColor { get; }
		public abstract string Icon { get; }
		public abstract string CalendarColor { get; }

		#endregion

		#region Private Instantiation

		private sealed class FallSE : Season
		{
				public FallSE() : base($"{nameof(Id.Fall)}", Id.Fall) { }
				public override string Type => "Equinox";
				public override string BadgeColor => "badge-warning";
				public override string Icon => "fab fa-canadian-maple-leaf";
				public override string CalendarColor => CalendarColors.Warning;
		}
		private sealed class WinterSE : Season
		{
				public WinterSE() : base($"{nameof(Id.Winter)}", Id.Winter) { }
				public override string Type => "Solstice";
				public override string BadgeColor => "badge-primary";
				public override string Icon => "fas fa-snowflake";
				public override string CalendarColor => CalendarColors.Primary;
		}
		private sealed class SpringSE : Season
		{
				public SpringSE() : base($"{nameof(Id.Spring)}", Id.Spring) { }
				public override string Type => "Equinox";
				public override string BadgeColor => "badge-success";
				public override string Icon => "fas fa-cloud-sun-rain";
				public override string CalendarColor => CalendarColors.Success;
		}
		private sealed class SummerSE : Season
		{
				public SummerSE() : base($"{nameof(Id.Summer)}", Id.Summer) { }
				public override string Type => "Solstice";
				public override string BadgeColor => "badge-danger";
				public override string Icon => "far fa-sun";
				public override string CalendarColor => CalendarColors.Danger;
		}
		private sealed class FallEOYSE : Season
		{
				public FallEOYSE() : base("Fall (EOY)", Id.FallEOY) { }
				public override string Type => "Equinox";
				public override string BadgeColor => "badge-warning";
				public override string Icon => "fab fa-canadian-maple-leaf";
				public override string CalendarColor => CalendarColors.Warning;
		}
		#endregion

}

public static class CalendarColors
{
	//https://www.color-hex.com/color/f57f17
	public const string Forest = "#357cd2";
	public const string Blue = "#0b5394";  //00008b
	public const string DarkBlue = "#00008b";
	public const string Olive = "#7fa900";  // 808000
	public const string Clay = "#ea7a57";
	public const string Turqoise = "#00bdae";
	public const string Pumpkin = "#f57f17";
	public const string Warning = "#ffc107";  // Fall
	public const string Dark = "#343a40";
	public const string Info = "#17a2b8";
	public const string Primary = "#007bff";  // Winter
	public const string Success = "#28a745";  // Spring
	public const string Danger = "#dc3545";   // Summer 
	public const string Unknown = "#1aaa55";  //
}