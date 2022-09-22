
# BLZ010: Components (Rendering Errors)
- https://www.youtube.com/watch?v=P1C6ad18XgE
- how to handle view services exceptions

8:29: 

---

## `IsUserAuthoirized`

```csharp

using System.Security.Claims;

	
#region Constructor and DI
	private readonly AuthenticationStateProvider AuthenticationStateProvider;
	// private readonly ILogger Logger; private readonly IParashaRepository db;

	public ParashaService(
		AuthenticationStateProvider authenticationStateProvider) // , IParashaRepository parashaRepository, ILogger<ParashaService> logger
	{
		AuthenticationStateProvider = authenticationStateProvider;
		// db = parashaRepository; 	Logger = logger;
		
	}
#endregion

/*
  Usage Example

	if (!IsUserAuthoirized(vm.EMail, id, user))
	{
		LogExceptionMessage = $"Inside {nameof(Summary)}, logged in user:{vm.EMail} lacks authority for id={id}";
		Logger.LogWarning(LogExceptionMessage);
		UserInterfaceMessage += "User not authorized";
		throw new UserNotAuthoirizedException(UserInterfaceMessage);
	}
*/
	private bool IsUserAuthoirized(string registrationEmail, int id, ClaimsPrincipal user)
	{
		string userEmail = user.GetUserEmail();
		if (userEmail == registrationEmail) { return true; }

		if (user.RoleHasAdminOrSukkot())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
```

 # Notes on Exceptions
	-  http://blog.abodit.com/2010/03/using-exception-data-to-add-additional-information-to-an-exception/
 
```csharp
// ...
	catch (RegistratationException e) when (e.Data != null)
	foreach (DictionaryEntry de in e.Data)
		Console.WriteLine("    Key: {0,-20}      Value: {1}", 
											 "'" + de.Key.ToString() + "'", de.Value);
```

# Interface examples

```csharp
	Task<vwRegistration> Details(int id, ClaimsPrincipal user, bool showPrintInstructionMessage = false);
	Task<vwRegistration> DeleteConfirmation(int id, ClaimsPrincipal user);

	Task<int> DeleteConfirmed(int id);
	Task<RegistrationSummary> Summary(int id, ClaimsPrincipal user);
	Task<IndexVM> GetRegistrationStep();
	Task<int> AddHouseRulesAgreementRecord(string email, string timeZone);
```
