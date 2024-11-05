using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P099_DTO.Database;
using P099_DTO.Dto;
using P099_DTO.Models;
using P099_DTO.Services;

namespace P099_DTO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly IFakeTodoDatabase _fakeTodoDatabase;
    private readonly ITodoMapper _todoMapper;
    public TodoController(IFakeTodoDatabase fakeTodoDatabase, ITodoMapper todoMapper)
    {
        _fakeTodoDatabase = fakeTodoDatabase;
        _todoMapper = todoMapper;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var todos = _fakeTodoDatabase.GetTodoItems();
        var dto = _todoMapper.Map(todos);
        return Ok(dto);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var todo = _fakeTodoDatabase.GetTodoItem(id);
        if (todo == null)
        {
            return NotFound();
        }
        var dto = _todoMapper.Map(todo);
        return Ok(dto);
    }

    [HttpPost]
    public IActionResult Post([FromBody] TodoItemRequest req)
    {
        var todo = _todoMapper.Map(req);
        _fakeTodoDatabase.AddTodoItem(todo);
        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);

    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] TodoItemRequest req)
    {
        var todo = _fakeTodoDatabase.GetTodoItem(id);
        if (todo == null)
        {
            return NotFound();
        }
        _todoMapper.Project(todo, req);
        _fakeTodoDatabase.UpdateTodoItem(todo);
        return NoContent();
    }
}