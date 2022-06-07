# Task List

 `#`  | Done | Description 
 ----- | :-: | ----------- 
 005   |     | john-006-remove-multieditform-add-prettyblazor
 005   |     | john-005-ShabbatWeek-Chooser 
 004   |     | john-004-add-WeeklyVideoAddFormEdit
 003   | ✓  | add FluentValidation
 002   | ✓  | multi `EditForm`
 001   | ✓  | fix startup and app setting configuration


### 005-ShabbatWeek-Chooser 

1. When Index (of MultiEditForm) opens, it's components will not be shown
2. Make a Component ShabbatWeekChooser

Sequence of Events
1. MultiEditForm/Index.razor is opened
2. All the components except ShabbatWeekChooser are hidden
3. In ShabbatWeekChooser during open a list of the weeks are gotten 
- 1. Happy Path
    - the user selects one of the rows and calls back to Index
- 2. Override WeekCount
    - the user ignores the default rows that were selected and requeries the list by changing the WeekCount and clicking <kbd>Requery</kbd>
    - the user selects one of those rows and Happy Path follows.
- 3. Exception Path
    - a database error occurs and 
    - the user selects one of those rows and Happy Path follows.
4.

**ShabbatWeekChooser.razor**



```html
<div class="row col-12 my-2">
	Rows:
	<input @oninput="OnEmailChanged"
				 required
				 type="text"
				 value="@Email" />
</div>
```

Have it return a Tuple like value object
- Status Call: 1=Ok, 2=No Rows Found, 3=Database Errors
- SelectedShabbatWeek?
- ErrorMsg

```csharp
//Status, ErrorMsg
Task<Tuple<int, ShabbatWeek?, string>> GetShabbatWeek();
```

  
### 003

### 002
- Add Dapper and repository and all that goes with it.
- To Docs: added TaskList.md
- Added Pages/MultiEditForm and its entourage
- To Shared/, added BackHomeButton and LoadingComponent
- Test for `IX_WeeklyVideo_Unique: ShabbatWeekId ASC,	WeeklyVideoTypeId ASC` viloations
- Create a Wizard / Flowchart
- Maybe implement https://github.com/ardalis/Result

# Future Tasks
- Port (I.e.) move the Multi Add form to a GitHub repository. 
  - Use MudBlazor solution and make it about



