# Different Ways to Communicate Between Components

- [3 Ways to Communicate Between Components in Blazor](https://chrissainty.com/3-ways-to-communicate-between-components-in-blazor/)

## 1. EventCallbacks

> `EventCallback` and `EventCallback<T>` give us a better way to define component callbacks over using `Action` or `Func`.
> With `EventCallback`, the `StateHasChanged` call is made for you, automatically by the framework. 
> You can also provide a `synchronous` or `asynchronous` method to an EventCallback without having to make any code changes.

**Child component**
the child component exposes an `EventCallback` parameter like so...

```html
<!-- -->
<button @onclick="@(() => OnClick.InvokeAsync("Hello from ChildComponent"))">
  Click me
</button>
```

```csharp
[Parameter] public EventCallback<string> OnClick { get; set; }
```

**Parent component**
The parent component registered, in this case `ClickHandler` method, with the child component.

```html
<!-- -->
<ChildComponent OnClick="ClickHandler"></ChildComponent>
```

```csharp
// codify what the Func method does
void ClickHandler(string newMessage)
{
  message = newMessage;
}
```

## 2. Cascading Values
> Cascading values and parameters are a way to pass a value from a component to all of its descendants without having to use traditional component parameters.
- [More Details](https://chrissainty.com/understanding-cascading-values-and-cascading-parameters/)
> This makes them a great option when building UI controls which need to manage some common state. 
> One prominent example is Blazors form and validation components. 
> The `EditForm` component cascades a `EditContext` value to all the controls in the form. 
> This is used to coordinate **validation** and invoke form **events**.

#### parts of the Blazor framework
- [`CascadingValue`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.cascadingvalue-1?view=aspnetcore-6.0) is a class and is used with the [`CascadingParameter`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.cascadingparameterattribute) attribute.
- [`ChildContent`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.layoutview.childcontent?view=aspnetcore-6.0#microsoft-aspnetcore-components-layoutview-childcontent) if of the type `RenderFragment`
- [`RenderFragment`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.renderfragment?view=aspnetcore-6.0) It represents a segment of UI content, implemented as a delegate that writes the content to a RenderTreeBuilder class.


#### `CascadingValue` and `CascadingParameter` are part of the Blazor framework
- see  [docs](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-6.0) 
  - see also **Cascade multiple values** and **Passing data across a component hierarchy**.
- see also2 [Understanding Cascading Values & Cascading Parameters](https://chrissainty.com/understanding-cascading-values-and-cascading-parameters/) by Chris Sainty
  - Multiple Cascading Parameters, By Type, By Name, Updating Cascading Values, Using Events, Using Complex Types, Updating Values


An ancestor component provides a `CascadingValue` component, which wraps a subtree of a component hierarchy and supplies a single value to all of the components within its subtree.

In the example below, the value that is cascaded is the `TabContainer` component itself because of `this`.

**Tab Container**
```html
<h1>@SelectedTab</h1>
<CascadingValue Value="this">
    @ChildContent
</CascadingValue>
```

```csharp
  [Parameter] public RenderFragment ChildContent { get; set; }

  public string SelectedTab { get; private set; }

  public void SetSelectedTab(string selectedTab)
  {
      SelectedTab = selectedTab;
      StateHasChanged();
  }
```


**Tab Component**
- interesting that the `div` has an `onclick` event
```html
<div @onclick="SetSelectedTab">
  @Title @(TabContainer.SelectedTab == Title ? "Selected" : "")
</div>
```

```csharp
[CascadingParameter] TabContainer TabContainer { get; set; }
[Parameter] public string Title { get; set; }

void SetSelectedTab()
{
  TabContainer.SetSelectedTab(Title);
}
```

## 3. State Container

[Managing App State with Redux Episode 74 App State Management using Fluxor, a Flux/Redux library for Blazor](https://youtu.be/Vn6dKN_hTrs)
- [Code](https://blazorroadshow.azurewebsites.net/blazortrainfiles/FluxorDemo.zip)