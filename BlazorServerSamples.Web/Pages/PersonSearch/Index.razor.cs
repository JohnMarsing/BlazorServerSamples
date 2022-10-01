namespace BlazorServerSamples.Web.Pages.PersonSearch;

public partial class Index
{
	private bool IsDisabled = true;
	private bool IsDisabledMulti;

	private List<Person> People = new List<Person>();

	private Person SelectedPerson;
	private Person SelectedPersonNull;

	private IList<Person> SelectedPeople; // Used only by Multi-select

	private IList<Person> SelectedPeopleWithNotFoundTemplate; 

	private FormValidation FormModel = new FormValidation();

	protected override void OnInitialized()
	{
		People = PopulatePeople();
		SelectedPerson = new Person(12, "Charles", "The Great", 66, "Belgium");
	}

	private async Task<IEnumerable<Person>> GetPeopleLocal(string searchText)
	{
		return await Task.FromResult(People
			.Where(x => x.Firstname.ToLower()
			.Contains(searchText.ToLower()))
			.ToList());
	}

	private void HandleFormSubmit()  // Used only by Form
	{
		Console.WriteLine("Form Submitted Successfully!");
	}

	#region Binding to different type
	private int? SelectedPersonId;
	public int? ConvertPerson(Person person)
	{
		return person?.Id;
	}

	public Person LoadSelectedPerson(int? id)
	{
		return People.FirstOrDefault(p => p.Id == id);
	}
	#endregion


	#region Multi-select - Adding items on empty search result
	private readonly Random _random = new Random();
	private Task<Person> ItemAddedMethod(string searchText)
	{
		var randomPerson = People[_random.Next(People.Count - 1)];
		var newPerson = new Person(_random.Next(1000, int.MaxValue), searchText, randomPerson.Lastname, _random.Next(10, 70), randomPerson.Location);
		People.Add(newPerson);
		return Task.FromResult(newPerson);
	}
	#endregion

	private List<Person> PopulatePeople()
	{
		List<Person> p = new List<Person>();
		p.AddRange(new List<Person>() {
						new Person() { Id = 1, Firstname = "Martelle", Lastname = "Cullon" },
						new Person() { Id = 2, Firstname = "Zelda", Lastname = "Abrahamsson" },
						new Person() { Id = 3, Firstname = "Benedetta", Lastname = "Posse" },
						new Person() { Id = 4, Firstname = "Benoite", Lastname = "Gobel" },
						new Person() { Id = 5, Firstname = "Charlot", Lastname = "Fullicks" },
						new Person() { Id = 6, Firstname = "Vinson", Lastname = "Turbat" },
						new Person() { Id = 7, Firstname = "Lenore", Lastname = "Malam" },
						new Person() { Id = 8, Firstname = "Emanuele", Lastname = "Kolakovic" },
						new Person() { Id = 9, Firstname = "Rosalyn", Lastname = "Mackin" },
						new Person() { Id = 10, Firstname = "Yanaton", Lastname = "Krishtopaittis" },
						new Person() { Id = 11, Firstname = "Frederik", Lastname = "McGeachie" },
						new Person() { Id = 12, Firstname = "Parrnell", Lastname = "Ramsby" },
						new Person() { Id = 13, Firstname = "Coreen", Lastname = "McGann" },
						new Person() { Id = 14, Firstname = "Kyle", Lastname = "Coster" },
						new Person() { Id = 15, Firstname = "Evangelia", Lastname = "Bowker" },
						new Person() { Id = 16, Firstname = "Angeli", Lastname = "Collihole" },
						new Person() { Id = 17, Firstname = "Bill", Lastname = "Lawther" },
						new Person() { Id = 18, Firstname = "Kore", Lastname = "Reide" },
						new Person() { Id = 19, Firstname = "Tracy", Lastname = "Gwinnell" },
						new Person() { Id = 20, Firstname = "Lazaro", Lastname = "Partington" },
						new Person() { Id = 21, Firstname = "Doretta", Lastname = "Aingell" },
						new Person() { Id = 22, Firstname = "Olvan", Lastname = "Andraud" },
						new Person() { Id = 23, Firstname = "Templeton", Lastname = "Chetwynd" },
						new Person() { Id = 24, Firstname = "Daile", Lastname = "Kelsow" },
						new Person() { Id = 25, Firstname = "Marcie", Lastname = "Brearty" },
						new Person() { Id = 26, Firstname = "Irwinn", Lastname = "Lilian" },
						new Person() { Id = 27, Firstname = "Niki", Lastname = "Moreland" },
						new Person() { Id = 28, Firstname = "Honey", Lastname = "Waddup" },
						new Person() { Id = 29, Firstname = "Amber", Lastname = "Hoopper" },
						new Person() { Id = 30, Firstname = "Delilah", Lastname = "Dougary" },
						new Person() { Id = 31, Firstname = "Tory", Lastname = "Ovington" },
						new Person() { Id = 32, Firstname = "Doralin", Lastname = "Conrard" },
						new Person() { Id = 33, Firstname = "Eugene", Lastname = "Custard" },
						new Person() { Id = 34, Firstname = "Corella", Lastname = "Peotz" },
						new Person() { Id = 35, Firstname = "Chris", Lastname = "Rayne" },
						new Person() { Id = 36, Firstname = "Alexandro", Lastname = "Kwietek" },
						new Person() { Id = 37, Firstname = "Selie", Lastname = "Tenwick" },
						new Person() { Id = 38, Firstname = "Corliss", Lastname = "Haensel" },
						new Person() { Id = 39, Firstname = "Misti", Lastname = "Jikylls" },
						new Person() { Id = 40, Firstname = "Rosaline", Lastname = "Jephson" },
						new Person() { Id = 41, Firstname = "Irene", Lastname = "Farnsworth" },
						new Person() { Id = 42, Firstname = "Dominique", Lastname = "O'Shiels" },
						new Person() { Id = 43, Firstname = "Mellie", Lastname = "Cyson" },
						new Person() { Id = 44, Firstname = "Madelena", Lastname = "Chin" },
						new Person() { Id = 45, Firstname = "Charlotte", Lastname = "Clixby" },
						new Person() { Id = 46, Firstname = "Samara", Lastname = "Shavel" },
						new Person() { Id = 47, Firstname = "Brod", Lastname = "Kitt" },
						new Person() { Id = 48, Firstname = "Maridel", Lastname = "Dalley" },
						new Person() { Id = 49, Firstname = "Wini", Lastname = "Hundley" },
				});
		return p;
	}

}
