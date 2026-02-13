namespace TaskList.Core.Dtos.Models;

public class QueryParameters
{
    public string? SortByDateDirection { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}
