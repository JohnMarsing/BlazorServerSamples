using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorServerSamples.Web.Pages.BlazoredToast.Parasha.Data;

namespace BlazorServerSamples.Web.Pages.BlazoredToast.Parasha.Services;

public interface IParashaService
{
	string UserInterfaceMessage { get; set; }
	//Task<vwRegistration> Details(int id, ClaimsPrincipal user, bool showPrintInstructionMessage = false);
	//Task<vwRegistration> DeleteConfirmation(int id, ClaimsPrincipal user);

	//Task<int> DeleteConfirmed(int id);
	//Task<RegistrationSummary> Summary(int id, ClaimsPrincipal user);
	//Task<IndexVM> GetRegistrationStep();
	//Task<int> AddHouseRulesAgreementRecord(string email, string timeZone);
}

public class ParashaService : IParashaService
{
	public string UserInterfaceMessage { get; set; } = "";

	#region Constructor and DI
	private readonly IParashaRepository db;
	private readonly ILogger Logger;
	//private readonly AuthenticationStateProvider AuthenticationStateProvider;

	public ParashaService(
		IParashaRepository parashaRepository, ILogger<ParashaService> logger) // , AuthenticationStateProvider authenticationStateProvider
	{
		db = parashaRepository;
		Logger = logger;
		//AuthenticationStateProvider = authenticationStateProvider;
	}
	#endregion
}
