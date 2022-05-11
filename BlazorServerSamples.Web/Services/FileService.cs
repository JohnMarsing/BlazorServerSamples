namespace BlazorServerSamples.Web.Services;

using BlazorServerSamples.Web.Domain;
using BlazorServerSamples.Web.Settings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;

public interface IFileService
{
	string ReadFromFile();
	void SaveToFile(List<ToDoItem> toDoItems);
}

public class FileService : IFileService
{
	#region Constructor and DI
	private readonly IConfiguration _configuration;
	private readonly ILogger Logger;
	private readonly IOptions<SampleDataFiles> SampleDataFiles;
	public FileService(IConfiguration configuration, ILogger<FileService> logger, IOptions<SampleDataFiles> sampleDataFiles)
	{
		_configuration = configuration;
		Logger = logger;
		SampleDataFiles = sampleDataFiles;
	}
	#endregion

	// ToDo: put both of these in a Try Catch
	// ToDo: Add tuples to handle returning the happy path or return an error
	public string ReadFromFile()
	{
		Logger.LogDebug(string.Format("Inside {0}", nameof(FileService) + "!" + nameof(ReadFromFile)));
		return File.ReadAllText(SampleDataFiles.Value.ToDoItems);
	}

	public void SaveToFile(List<ToDoItem> toDoItems)
	{
		Logger.LogDebug(string.Format("Inside {0}", nameof(FileService) + "!" + nameof(SaveToFile)));
		string json = JsonConvert.SerializeObject(toDoItems);
		System.IO.File.WriteAllText(SampleDataFiles.Value.ToDoItems, json);	
	}
}
