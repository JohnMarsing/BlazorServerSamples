# Pass data from child to parent
- [source Pragim Tech](https://www.pragimtech.com/blog/blazor/pass-data-from-child-to-parent-component-in-blazor/)
  - [YouTube](https://www.youtube.com/watch?v=Kb5bWCuQ9bQ)
  - [Slides and Notes](https://www.pragimtech.com/blog/blazor/blazor-eventcallback/)
  - [Pragim Tech Blazor Blog](https://www.pragimtech.com/courses/blazor-tutorial-for-beginners/)

- Uses `EventCallback`, `InvokeAsync`

> When the Delete button (which is found in the child component) is clicked, the respective employee must be deleted and the employee card should be removed.

### Child Component

**DisplayEmployee.razor**
```html
<h3>Employee Detail</h3>

<button type="button" class="btn btn-danger m-1" 
        @onclick="Delete_Click">
    Delete
</button>
```

**DisplayEmployee.razor.cs**
```csharp
[Parameter]
public EventCallback<int> OnEmployeeDeleted { get; set; }

[Inject]
public IEmployeeService EmployeeService { get; set; }

[Inject]
public NavigationManager NavigationManager { get; set; }  // WHY IS THIS HERE???

protected async Task Delete_Click()
{
    await EmployeeService.DeleteEmployee(Employee.EmployeeId);
    await OnEmployeeDeleted.InvokeAsync(Employee.EmployeeId);
    //NavigationManager.NavigateTo("/", true);  WHY IS THIS HERE???
}
```

### Parent Page

**EmployeeList.razor**
```html
@foreach (var employee in Employees)
{
  <DisplayEmployee Employee="employee" ShowFooter="ShowFooter"
                   OnEmployeeSelection="EmployeeSelectionChanged"
                   OnEmployeeDeleted="EmployeeDeleted"></DisplayEmployee>
}
```

**EmployeeList.razor.cs**
```csharp
protected async Task EmployeeDeleted()
{
    Employees = (await EmployeeService.GetEmployees()).ToList();
}
```



