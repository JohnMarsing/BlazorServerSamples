# Complex Object Binding
- [source Csharp Fritz](https://gist.github.com/csharpfritz/c4dcfcc731826822e0764728bbd9d88c)
- Uses `JsonSerializer.Deserialize`, part of  `System.Text.Json`

### Component

**NavUnorderedList.razor**
```html
<h3>Nav Unordered List</h3>

@if (NavItems != null && NavItems.Count() > 0)
{
<ul>
  @foreach (var item in NavItems)
  {
    <li>@item.Title</li>
  }
</ul>
} else
{
  <span>Loading...</span>
}
```

**NavUnorderedList.razor.cs**
```csharp
  [Parameter]
  public IEnumerable<BlazorApp2.Data.Nav> NavItems { get; set; }
```


### Page

**Index.razor**
```html
@page "/"
@using System.Text.Json
<h1>Index</h1>
<NavUnorderedList NavItems="TheItems"></NavUnorderedList>
```

**Index.razor.cs**
```csharp
  public List<BlazorApp2.Data.Nav> TheItems { get; set; }
  private string theJson = "...";

  protected override Task OnInitializedAsync()
  {
    var theNavs = JsonSerializer.Deserialize<BlazorApp2.Data.Nav[]>(theJson, new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive=true
    });
    TheItems = theNavs;
    return base.OnInitializedAsync();
  }
```



### Model

**Nav.cs**
```csharp
  public class Nav
  {
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
  }
```

---

# Serialize and deserialize JSON in a Blazor application
- [Source](https://www.syncfusion.com/faq/blazor/general/how-do-i-serialize-and-deserialize-json-in-a-blazor-application)

# Resources
Not sure, but this is nothing special about Blazor, it could be Razor Pages as well

https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0

https://www.learnrazorpages.com/web-api