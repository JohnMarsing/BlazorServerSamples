using BlazorServerSamples.Web.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlazorServerSamples.Web.Pages.ToDoSortJson;
using BlazorServerSamples.Web.Services;
using BlazorServerSamples.Web.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

//		services.AddDataStores();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddSingleton<ILinkService, LinkService>();
//		services.AddCustomAuthentication(Configuration);

builder.Services.AddOptions();
builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
builder.Configuration.GetSection("SampleDataFiles").Get<SampleDataFiles>();

//var builder = WebApplication.CreateBuilder(args);

string LmmConnectionDbString = builder.Configuration.GetConnectionString("LivingMessiah");
//string connLmmString = builder.Configuration.GetSection("AppSettings");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
