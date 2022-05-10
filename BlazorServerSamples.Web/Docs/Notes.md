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


# Bugs
## Bug 001 AppSettings NoWorky

Inside `Program.cs`, I Can't figure out how to read the sections in `appsettings.json` and save them in the class `AppSettings.cs` so that it can be referenced latter on.
I could do this before in .Net 5 when there was a Startup.cs class but this got changed.
I included notes on how I, in the past, extracted a connection string as well


```csharp

//builder.Services.AddOptions
//IWebHostEnvironment environment = builder.Environment;
//var settings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();


/*
ConfigurationManager configuration = builder.Configuration;
configuration.AddConfiguration
builder.Services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));
builder.Services.Configure<SampleDataFiles>(options => Configuration.GetSection("SampleDataFiles").Bind(options));

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration["Redis"];
});

 */

```

### Refrences

- LivingMessiah.Web\Startup.cs

**Startup.cs**
```csharp
namespace LivingMessiah.Web;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services)
	{
		//...
		services.AddCustomAuthentication(Configuration);
		services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));
		services.Configure<SukkotSettings>(options => Configuration.GetSection("SukkotSettings").Bind(options));
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		//...	
		app.UseSerilogRequestLogging();
		//...			
	}
}
```

**Program.cs**
- I added this here because I want to use Serilog to stuff in `Startup.cs`
```csharp
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LivingMessiah.Web;

public class Program
{
		public static void Main(string[] args)
		{
				string appSettingJson;
				if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
				{
						appSettingJson = "appsettings.Development.json";
				}
				else
				{
						appSettingJson = "appsettings.Production.json";
				}

				var configuration = new ConfigurationBuilder()
					.AddJsonFile(appSettingJson)  // "appsettings.json"
					.Build();

				Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(configuration)
					.CreateLogger();
				Log.Warning($"Inside {nameof(Program)}; testing that this message gets saved to the Serilog console and file sinks. ASPNETCORE_ENVIRONMENT: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
				try
				{
						Log.Information("Application Starting Up"); // Note 1
						CreateHostBuilder(args).Build().Run();
				}
				catch (Exception ex)
				{
						Log.Fatal(ex, "The application failed to start correctly"); // Total fale
				}
				finally
				{
						Log.CloseAndFlush(); // Note 2
				}
		}
		/*
		Note 1: because we are in static void Main, we have to use the static keyword Log.Information not LogInformation
		        i.e. we can't use the ILogger right now we must use the Serilog logger
		Note 2: If you have any log messages that are pending, then this will make sure they are written.
	 */

		public static IHostBuilder CreateHostBuilder(string[] args) =>
				Host.CreateDefaultBuilder(args)
						.UseSerilog()
						.ConfigureWebHostDefaults(webBuilder =>
						{
								webBuilder.UseStartup<Startup>();
						});
}

```


**ApplicationSettings.razor**
- LivingMessiah.Web\Pages\Admin\Dashboard\ApplicationSettings.razor
```html
@using Microsoft.Extensions.Options
@using LivingMessiah.Web.Settings

@inject IOptions<AppSettings> AppSettings

<p class="card-title">GoogleAnalytics: @AppSettings.Value.GoogleAnalytics</p>

```


**Index.razor.cs.cs**
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


**AppSettings.cs**
- LivingMessiah.Web\Settings\
```csharp
public class AppSettings
{
		public int YearId { get; set; }
		public string SiteShortTitle { get; set; }
		public string SiteTitle { get; set; }
		public string GoogleAnalytics { get; set; }
		public bool ShabbatServiceLoadQuickly { get; set; }
		public bool ShowCurrentWeeklyVideos { get; set; }
}

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






