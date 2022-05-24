namespace BlazorServerSamples.Data;

using System.Data.SqlClient;

public static class Exceptions
{
	const int UniqueIndexViolation2601 = 2601; // Cannot insert duplicate key row in object '<Object Name>' with unique index '<Index Name>'
	const int PrimaryKeyViolation2627 = 2627; // Violation of PRIMARY KEY constraint

	public static bool IsUniqueKeyViolation(this SqlException ex)
	{
		return ex.Errors.Cast<SqlError>().Any(e => e.Class == 14 && (e.Number == UniqueIndexViolation2601));
	}

	public static bool IsPrimaryKeyViolation(this SqlException ex)
	{
		return ex.Errors.Cast<SqlError>().Any(e => e.Class == 14 && (e.Number == PrimaryKeyViolation2627));
	}

	public static bool IsPrimaryKeyOrUniqueindexViolation(this SqlException ex)
	{
		return ex.Errors.Cast<SqlError>().Any(e => e.Class == 14 && (e.Number == PrimaryKeyViolation2627) || e.Number == UniqueIndexViolation2601);
	}

}
