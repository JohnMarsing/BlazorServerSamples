# FluentValidation


https://stackoverflow.com/questions/71668679/mudblazor-select-with-multiselect-and-fluentvalidation-for-expression

#### NotNull vs NotEmpty
- NotEmpty Validator: Ensures that the specified property is not null, an empty string or whitespace (or the default value for value types, e.g., 0 for int). 
- NotNull Validator: Ensures **ONLY** that the specified property is not null.
- [docs](https://docs.fluentvalidation.net/en/latest/built-in-validators.html?highlight=notempty#notempty-validator)

#### [Custom validation logic on WASM client AND server with Blazor](https://jonhilton.net/blazor-client-server-validation-with-fluentvalidation/) 
- Instant validation of a just entered email is already in use.
- This example recognizes security issues

**Shared\Account\SignUp.cs** uses **`MustAsync`** 

```csharp
public class SignUpValidator : AbstractValidator<SignUp>
{
    private readonly IValidateEmail _validateEmail;

    public SignUpValidator(IValidateEmail validateEmail)
    {
        _validateEmail = validateEmail;

        RuleFor(x => x.Email)
            .NotEmpty()
            .MustAsync(BeUnique).WithMessage("Email already registered");
    }

    private async Task<bool> BeUnique(string email, CancellationToken token)
    {
        return await _validateEmail.CheckIfUnique(email, token);
    }
}
```

**`IValidateEmail.cs`** uses `CancellationToken`

```csharp
public interface IValidateEmail
{
    Task<bool> CheckIfUnique(string email, CancellationToken cancellationToken);
}
```

##### Two Implementations 

**Client project**
```csharp
public class ValidateEmail : IValidateEmail
{
    private readonly HttpClient _http;

    public ValidateEmail(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<bool> CheckIfUnique(string email, CancellationToken token)
    {
        var requestUri = $"account?email={email}";
        
        var existingAccounts = await 
            _http.GetFromJsonAsync<Search.Model>(requestUri, token);
        
        return !existingAccounts.Accounts.Any();
    }
}


// Startup.cs
  builder.Services.AddTransient<IValidateEmail, ValidateEmail>();

```

**Server project**
```csharp
public class ValidateEmail : IValidateEmail
{
    private readonly FakeSearch _fakeSearch;

    public ValidateEmail(FakeSearch fakeSearch)
    {
        _fakeSearch = fakeSearch;
    }
    
    public async Task<bool> CheckIfUnique(string email, CancellationToken cancellationToken)
    {
        var existingAccounts = _fakeSearch.Handle(new Search.Query {Email = email});
        return !existingAccounts.Accounts.Any();
    }
}

// Startup.cs
  services.AddScoped<IValidateEmail, ValidateEmail>();

```

> Check blog for more code

---

#### [RuleSets](https://docs.fluentvalidation.net/en/latest/rulesets.html?highlight=IncludeRuleSets#rulesets)
> RuleSets allow you to group validation rules together which can be executed together as a group whilst ignoring other rules:

Example, see **`Person.cs`**  below 

### `Blazored.FluentValidation`
- Dependent on [FluentValidation](https://docs.fluentvalidation.net/)
- [GitHub](https://github.com/Blazored/FluentValidation).  
  - Read up on [finding-validators](https://github.com/Blazored/FluentValidation#finding-validators)
  -  If you only wish the component to get validators from DI, set the value to true and assembly scanning will be skipped.
  -  `<FluentValidationValidator DisableAssemblyScanning="@true" />`

##### [Blazor Server Sample](https://github.com/Blazored/FluentValidation/tree/main/samples/BlazorServer)
- uses DI not reflection.
- [GitHub](https://github.com/Blazored/FluentValidation/blob/main/samples/BlazorServer/Pages/Index.razor)



**Index.razor**
```html
<EditForm Model="@Person" OnValidSubmit="@SubmitValidForm">
    <FluentValidationValidator @ref="_fluentValidationValidator" DisableAssemblyScanning="@true" />
    <ValidationSummary />
    <!-- ... -->
    <button type="submit">Save</button>

</EditForm>
<br />
<button @onclick="PartialValidate">Partial Validation</button>
```

**Index.razor.cs**
```csharp
  private FluentValidationValidator? _fluentValidationValidator;
  void PartialValidate()
  {
      Console.WriteLine($"Partial validation result : {_fluentValidationValidator?.Validate(options => options.IncludeRuleSets("Names"))}");
  }

  void SubmitValidForm()
  {
      Console.WriteLine("Form Submitted Successfully!");
  }

```

#### SharedModels class library Project
- This was used by both examples Blazor Server and WASM
**SharedModels.csproj**
```json
  <ItemGroup>
    <PackageReference Include="FluentValidation" Version="11.0.0" />
  </ItemGroup>
```

Shared\SharedModels\


**`Person.cs`** 
```csharp
using FluentValidation;

namespace SharedModels; 
public class Person
{
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public int? Age { get; set; }
  public string? EmailAddress { get; set; }
  public Address Address { get; set; } = new();
}

public class PersonValidator : AbstractValidator<Person>
{
  public PersonValidator()
  {
    RuleSet("Names", () =>
    {
      RuleFor(p => p.FirstName)
      .NotEmpty().WithMessage("You must enter your first name")
      .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters");

      RuleFor(p => p.LastName)
      .NotEmpty().WithMessage("You must enter your last name")
      .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters");
    });

    RuleFor(p => p.Age)
      .NotNull().WithMessage("You must enter your age")
      .GreaterThanOrEqualTo(0).WithMessage("Age must be greater than 0")
      .LessThan(150).WithMessage("Age cannot be greater than 150");

    RuleFor(p => p.EmailAddress)
      .NotEmpty().WithMessage("You must enter a email address")
      .EmailAddress().WithMessage("You must provide a valid email address")
      .MustAsync(async (email, _) => await IsUniqueAsync(email)).WithMessage("Email address must be unique").When(p => !string.IsNullOrEmpty(p.EmailAddress));

    RuleFor(p => p.Address).SetValidator(new AddressValidator());
  }

  private static async Task<bool> IsUniqueAsync(string email)
  {
      await Task.Delay(300);
      return email.ToLower() != "mail@my.com";
  }
}
```

- **`Address.cs`** not shown, [see](https://github.com/Blazored/FluentValidation/blob/main/samples/Shared/SharedModels/Address.cs)


### Comparison of Nuget packages

> FluentValidation does not provide integration with Blazor out of the box, 
> but there are several third party libraries you can use to do this...
- [Source](https://docs.fluentvalidation.net/en/latest/blazor.html)
- **NOTE**, don't use this [Chris Sainty blog](https://chrissainty.com/using-fluentvalidation-for-forms-validation-in-razor-components/), use his **Blazored.FluentValidation** instead (see comments section of blog).


 Package  		              | Stars / Downloads | Description 
 ----- 			                | :-:   | ----------- 
Blazored.FluentValidation   | 352 453k | https://github.com/Blazored/FluentValidation
Blazor-Validation	          | 116   | https://github.com/mrpmorris/blazor-validation
Accelist.FluentValidation.Blazor   | 204  | https://github.com/ryanelian/FluentValidation.Blazor
vNext.BlazorComponents.FluentValidation |   6    | https://github.com/Liero/vNext.BlazorComponents.FluentValidation



### Incorporate with MudBlazor
- https://mudblazor.com/components/form#using-fluent-validation
 


### ???
