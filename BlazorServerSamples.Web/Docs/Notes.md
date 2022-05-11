# ToDo

## Add a `GlobalUsings.cs` class
- See https://gunnarpeipman.com/global-usings/

Example
```csharp
global using System.Data;
global using System.Data.SqlClient;
global using ConsoleApp4.FileAccess;
```

For MVC and RP, instead of keeping usings in `_ViewImports.cs` make a global usings file.

```csharp
global using System.Diagnostics;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
```


**Index.razor.cs**
- LivingMessiah.Web\Pages\Calendar\
- Get AppSettings.YearId
```csharp

public partial class Index
{
using Microsoft.Extensions.Options;
using LivingMessiah.Web.Settings;

		[Inject]
		public IOptions<AppSettings> AppSettings { get; set; }

		protected override async Task OnInitializedAsync()
		{
				YearId = AppSettings.Value.YearId;
				Logger.LogDebug(string.Format("Inside Page: {0}, Class!Method: {1}, YearId:{2}", Page.Index, nameof(Index) + "!" + nameof(OnInitializedAsync), YearId));
```




### Dapper and ConnectionString

**`BaseRepositoryAsync.cs`**
- LivingMessiah.Data\BaseRepositoryAsync.cs

```csharp

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LivingMessiah.Data;

public abstract class BaseRepositoryAsync
{
	const string configationConnectionKey = "ConnectionStrings:LivingMessiah"; // Found in LivingMessiah.Web!appSetting.json

	private readonly IConfiguration config;
	protected readonly ILogger log;
	protected BaseRepositoryAsync(IConfiguration config, ILogger<BaseRepositoryAsync> logger)
	{
		this.config = config;
		this.log = logger;
	}

	protected async Task<T> WithConnectionAsync<T>(Func<IDbConnection, Task<T>> getData)
	{
		string connectionString = config[configationConnectionKey];
		string errMsg = "";

		try
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				string err = $"Inside {GetType().FullName}.{nameof(WithConnectionAsync)}; Connection string is null or empty.  configationConnectionKey={configationConnectionKey}";
				throw new ArgumentException(err);
			}

			using (var connect = new SqlConnection(connectionString))
			{
				await connect.OpenAsync();
				return await getData(connect);
			}
		}
		catch (TimeoutException ex) //...
		catch (SqlException ex)	//...
		catch (InvalidOperationException ex) //...
		catch (Exception ex) //...
		}
	}

	public string Sql { get; set; }
	public DynamicParameters Parms { get; set; }  // using Dapper; Note, only place dependent on Dapper
	public string SqlDump
	{
		get
		{
			string s = "";
			// ...
	}

```

### Example of a Repository implementing `BaseRepositoryAsync.cs`
- LivingMessiah.Web\Pages\Admin\AudioVisual\WeeklyVideosRepository.cs

**`WeeklyVideosRepository.cs`**
```csharp
using LivingMessiah.Data;

namespace LivingMessiah.Web.Pages.Admin.AudioVisual;

public interface IWeeklyVideosRepository
{
	string BaseSqlDump { get; }
	// Query 
	Task<List<ShabbatWeek>> GetShabbatWeekList(int top);
	// ...

	// Command
	Task<int> WeeklyVideoAdd(WeeklyVideoInsert dto);
	// ...
}


public class WeeklyVideosRepository : BaseRepositoryAsync, IWeeklyVideosRepository
{
	public WeeklyVideosRepository(IConfiguration config, ILogger<WeeklyVideosRepository> logger) : base(config, logger)
	{	}

	public string BaseSqlDump
	{
		get { return SqlDump; } // base.SqlDump
	}

	#region Query

	public async Task<List<WeeklyVideoTable>> GetWeeklyVideoTableList(int top = 9)
	{
		base.log.LogDebug(string.Format("Inside {0}, top={1}", nameof(WeeklyVideosRepository) + "!" + nameof(GetWeeklyVideoTableList), top));
		Parms = new DynamicParameters(new { Top = top });

		// Sql = base.Sql
		Sql = $@"
-- DECLARE @Top int = 3
SELECT 
-- ...
";
    // WithConnectionAsync = base.WithConnectionAsync
		return await WithConnectionAsync(async connection =>
		{
			var rows = await connection.QueryAsync<WeeklyVideoTable>(sql: Sql, param: Parms);
			// base.log.LogDebug(string.Format("...Sql {0}", Sql));
			return rows.ToList();
		});
	}
```

**`ServiceCollectionExtensions.cs`**
```csharp
namespace LivingMessiah.Web;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDataStores(this IServiceCollection services)
	{
		services
			.AddTransient<IWeeklyVideosRepository, WeeklyVideosRepository>()
		// ...
		return services;
	}
```



### `LivingMessiah.Web.csproj` references LivingMessiah.Data.csproj
- LivingMessiah.Web\LivingMessiah.Web.csproj
```
  <ItemGroup>
    <ProjectReference Include="..\LivingMessiah.Data\LivingMessiah.Data.csproj" />
		// ...
  </ItemGroup>
```






