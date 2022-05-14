
# Bugs List


 Num.  | Done | Description 
 ----- | :--:  | ----------- 
 001   | ✓ | AppSettings NoWorky       
 999   |  | XXXX    
 

#### Bug 001 AppSettings NoWorky

- [x] Done

Inside `Program.cs`, I Can't figure out how to read the sections in `appsettings.json` and save them in the class `AppSettings.cs` so that it can be referenced latter on.
I could do this before in .Net 5 when there was a Startup.cs class but this got changed.
I included notes on how I, in the past, extracted a connection string as well

**Closed**

- reverted back .Net 5 solution i.e. using `Startup.cs` and `ServiceCollectionExtensions.cs`
- If you read the comments of this blog [Upgrading a .NET 5 "Startup-based" app to .NET 6](https://andrewlock.net/exploring-dotnet-6-part-12-upgrading-a-dotnet-5-startup-based-app-to-dotnet-6/) there seems to be problems.

**Code Changes**

**1.** Added [Bootstrap Icons](https://icons.getbootstrap.com/), why is MS using open-icon? and used it the GitHub icon
```
<link rel="stylesheet" href="css/bootstrap/bootstrap.min.css" />
```

**2.**
- Added logging to `FileService.cs` and `ToDoService.cs`
- Injected Options to `FileService.cs`
```csharp
private readonly IOptions<SampleDataFiles> SampleDataFiles;
```

---

###### Handy Notes

 Key(s)      | Description 
 ----------- | ----------- 
 <kbd>Ctrl</kbd> + <kbd>Space</kbd> | checks and unchecks task list items.