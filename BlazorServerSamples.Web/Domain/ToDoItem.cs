using System;

namespace BlazorServerSamples.Web.Domain;

public class ToDoItem
{
	public Guid ID { get; set; }
	public string Description { get; set; } = String.Empty;
	public bool IsComplete { get; set; }
	public DateTime DateCreated { get; set; }
}
