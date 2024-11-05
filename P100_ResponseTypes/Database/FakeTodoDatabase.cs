using P100_ResponseTypes.Models;

namespace P100_ResponseTypes.Database;

public interface IFakeTodoDatabase
{
    void AddTodoItem(TodoItem todoItem);
    void DeleteTodoItem(int id);
    TodoItem? GetTodoItem(int id);
    List<TodoItem> GetTodoItems();
    void UpdateTodoItem(TodoItem todoItem);
    bool UserExists(string userId);
}
public class FakeTodoDatabase : IFakeTodoDatabase
{
    private readonly List<TodoItem> _todoItems = new List<TodoItem>
    {
        new TodoItem
        {
            Id = 1,
            Name = "Work",
            Content = "First todo content",
            EndDate = DateTime.Now.AddDays(1),
            UserId = "1"
        },
        new TodoItem
        {
            Id = 2,
            Name = "Home",
            Content = "Second todo content",
            EndDate = DateTime.Now.AddDays(2),
            UserId = "2"
        },
        new TodoItem
        {
            Id = 3,
            Name = "Work",
            Content = "Third todo content",
            EndDate = DateTime.Now.AddDays(3),
            UserId = "3"
        }
    };

    public List<TodoItem> GetTodoItems()
    {
        return _todoItems;
    }

    public TodoItem? GetTodoItem(int id)
    {
        return _todoItems.Find(x => x.Id == id);
    }

    public void AddTodoItem(TodoItem todoItem)
    {
        _todoItems.Add(todoItem);
    }

    public void UpdateTodoItem(TodoItem todoItem)
    {
        var index = _todoItems.FindIndex(x => x.Id == todoItem.Id);
        if (index != -1)
        {
            _todoItems[index] = todoItem;
        }
    }
    public void DeleteTodoItem(int id)
    {
        var index = _todoItems.FindIndex(x => x.Id == id);
        if (index != -1)
        {
            _todoItems.RemoveAt(index);
        }
    }

    public bool UserExists(string userId)
    {
        return _todoItems.Any(x => x.UserId == userId);
    }

}