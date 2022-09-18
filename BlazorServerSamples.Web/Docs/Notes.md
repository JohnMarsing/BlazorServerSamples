# Catchall Notes


https://chrissainty.com/building-a-simple-tooltip-component-for-blazor-in-under-10-lines-of-code/
https://chrissainty.com/auto-saving-form-data-in-blazor/
https://chrissainty.com/building-custom-input-components-for-blazor-using-inputbase/
https://chrissainty.com/page/2/

## Two buttons with spaces
```html
<div class="row">
  <div class="col-12 my-2">
    <button type="button" @onclick="LogErrorTest_ButtonClick"	class="btn btn-danger btn-sm ">
			<i class="fas fa-times"></i> Log Error Test
    </button>
    <button  @onclick="EmptyErrorLog_ButtonClick" class="btn btn-danger btn-sm">
      <i class="fas fa-times"></i> Empty Log
    </button>
  </div>
</div>
```

## Two Buttons on a Form
```html
<div class="row col-12 my-2">
  <div class="form-group">
    <button class="btn btn-warning btn-md" 
      @onclick=@(() => ClickPartialValidate("Select")) title="Partial validation for Select only">
      <i class="bi bi-check"></i> Validate 1
    </button>
    <button class="btn btn-warning btn-md" 
      @onclick=@(() => ClickPartialValidate("TitleOnly")) title="Partial validation for Title only">
      <i class="bi bi-check"></i> Validate 2
    </button>
  </div>
</div>
```

## Dealing with nullable-warnings
- [docs](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-warnings)
 

The Bang **`!`** is the null forgiving operator.

Examples
```csharp


[Inject] public ILogger<Index>? Logger { get; set; } 

protected override async Task OnInitializedAsync()
{
  Logger!.LogDebug(string.Format("Inside {0}", nameof(Index) + "!" + nameof(OnInitialized)));
  // do something
}

// another example
Console.WriteLine(message!.Length);

```


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
- BlazorServerSamples.Web\Pages\Calendar\
- Get AppSettings.YearId
```csharp

public partial class Index
{
using Microsoft.Extensions.Options;
using BlazorServerSamples.Web.Settings;

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
  const string configationConnectionKey = "ConnectionStrings:LivingMessiah"; // Found in BlazorServerSamples.Web!appSetting.json

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
    catch (SqlException ex)  //...
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
- BlazorServerSamples.Web\Pages\Admin\AudioVisual\WeeklyVideosRepository.cs

**`WeeklyVideosRepository.cs`**
```csharp
using LivingMessiah.Data;

namespace BlazorServerSamples.Web.Pages.Admin.AudioVisual;

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
  {  }

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
namespace BlazorServerSamples.Web;
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



### `BlazorServerSamples.Web.csproj` references LivingMessiah.Data.csproj
- BlazorServerSamples.Web\BlazorServerSamples.Web.csproj
```
  <ItemGroup>
    <ProjectReference Include="..\LivingMessiah.Data\LivingMessiah.Data.csproj" />
    // ...
  </ItemGroup>
```






