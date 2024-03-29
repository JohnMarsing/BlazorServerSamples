﻿namespace BlazorServerSamples.Web.Enums;
using Ardalis.SmartEnum;

public abstract class WeeklyVideoType : SmartEnum<WeeklyVideoType>
{
	#region Id's
	private static class Id
	{
		internal const int MainServiceEnglish = 1;
		internal const int MainServiceSpanish = 2;
		internal const int InDepthStudy = 3;
		internal const int TorahTuesday = 4;
	}
	#endregion

	#region  Declared Public Instances
	public static readonly WeeklyVideoType MainServiceEnglish = new MainServiceEnglishSE();
	public static readonly WeeklyVideoType MainServiceSpanish = new MainServiceSpanishSE();
	public static readonly WeeklyVideoType InDepthStudy = new InDepthStudySE();
	public static readonly WeeklyVideoType TorahTuesday = new TorahTuesdaySE();
	// SE=SmartEnum
	#endregion

	private WeeklyVideoType(string name, int value) : base(name, value) { }  // Constructor

	#region Extra Fields
	//public abstract string DivClass { get; }
	#endregion

	#region Private Instantiation

	private sealed class MainServiceEnglishSE : WeeklyVideoType
	{
		public MainServiceEnglishSE() : base("Main Service English", Id.MainServiceEnglish) { }
		//public override string DivClass => "d-sm-none";
	}

	private sealed class MainServiceSpanishSE : WeeklyVideoType
	{
		public MainServiceSpanishSE() : base("Main Service Spanish", Id.MainServiceSpanish) { }
	}

	private sealed class InDepthStudySE : WeeklyVideoType
	{
		public InDepthStudySE() : base("In-depth Study", Id.InDepthStudy) { }
	}

	private sealed class TorahTuesdaySE : WeeklyVideoType
	{
		public TorahTuesdaySE() : base("Torah Tuesday", Id.TorahTuesday) { }
	}

	#endregion

}