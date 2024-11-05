using P099_DTO.Dto;
using P099_DTO.Models;

namespace P099_DTO.Services;

public interface ITodoMapper
{
    IEnumerable<TodoItemResult> Map(IEnumerable<TodoItem> items);
    TodoItemResult Map(TodoItem item);
    TodoItem Map(TodoItemRequest req);
    void Project(TodoItem item, TodoItemRequest req);
}
public class TodoMapper : ITodoMapper
{
    public TodoItemResult Map(TodoItem item)
    {
        return new TodoItemResult
        {
            Id = item.Id,
            Name = item.Name,
            Content = item.Content,
            EndDate = item.EndDate,
            UserId = item.UserId
        };
    }
    public IEnumerable<TodoItemResult> Map(IEnumerable<TodoItem> items)
    {
        return items.Select(Map);
    }

    public TodoItem Map(TodoItemRequest req)
    {
        return new TodoItem
        {
            Name = req.Name,
            Content = req.Content,
            EndDate = req.EndDate,
            UserId = req.UserId
        };
    }

    public void Project(TodoItem item, TodoItemRequest req)
    {
        item.Name = req.Name;
        item.Content = req.Content;
        item.EndDate = req.EndDate;
        item.UserId = req.UserId;
    }
}