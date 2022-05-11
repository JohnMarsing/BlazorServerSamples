using BlazorServerSamples.Web.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorServerSamples.Web.Services;

public interface IToDoService
{
	List<ToDoItem> Get();
	ToDoItem Get(Guid ID);
	List<ToDoItem> Add(ToDoItem toDoItem);
	List<ToDoItem> Toggle(Guid id);
	List<ToDoItem> Delete(Guid ID);
}

public class ToDoService : IToDoService
{
	#region Constructor and DI
	private readonly IFileService _fileService;
	private List<ToDoItem> _toDoItems;
	private readonly ILogger Logger;


	public ToDoService(IFileService fileService, ILogger<ToDoService> logger)
	{
		_fileService = fileService;
		Logger = logger;
	}
	#endregion

	public List<ToDoItem> Get()
	{
		Logger.LogDebug(string.Format("Inside {0}", nameof(ToDoService) + "!" + nameof(Get)));
		string json = _fileService.ReadFromFile();
		_toDoItems = JsonConvert.DeserializeObject<List<ToDoItem>>(json);
		return _toDoItems;
	}

	public ToDoItem Get(Guid ID)
	{
		return _toDoItems.First(x => x.ID == ID);
	}

	public List<ToDoItem> Add(ToDoItem toDoItem)
	{
		_toDoItems.Add(toDoItem);
		_fileService.SaveToFile(_toDoItems);
		return _toDoItems;
	}

	public List<ToDoItem> Toggle(Guid ID)
	{
		var toDoItemToUpdate = Get(ID);

		if (toDoItemToUpdate != null)
		{
			toDoItemToUpdate.IsComplete = !toDoItemToUpdate.IsComplete;
			_fileService.SaveToFile(_toDoItems);
		}

		return _toDoItems;
	}

	public List<ToDoItem> Delete(Guid ID)
	{
		var toDoItemToRemove = Get(ID);

		if (toDoItemToRemove != null)
		{
			_toDoItems.Remove(Get(ID));
			_fileService.SaveToFile(_toDoItems);
		}

		return _toDoItems;
	}
}
