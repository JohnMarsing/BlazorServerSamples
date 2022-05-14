
namespace BlazorServerSamples.Domain;

public class zvwErrorLog
{
	public String ErrorProcedure { get; set; } = String.Empty;
	public Int32 ErrorLine { get; set; }
	public String ErrorMessage { get; set; } = String.Empty;
	public String HowLongAgoHMS { get; set; } = String.Empty;
	public Int32 ErrorLogID { get; set; }
	public String ErrorTime2 { get; set; } = String.Empty;
}
