using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.TodoList.Commands;
    using Application.TodoList.Models;
    using Application.TodoList.Queries;
    using Domain.Todo;
    using MediatR;

    public class TodoListController : ApiControllerBase
    {
        private readonly ISender _mediator;
        private readonly ILogger<TodoListController> _logger;

        public TodoListController(ISender mediator, ILogger<TodoListController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet("todolist")]
        public async Task<ActionResult<PaginatedList<TodoListDto>>> GetTodoLists([FromQuery] GetTodoListsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("todolist/{listId:int}")]
        public async Task<ActionResult<TodoListDto>> GetTodoListById(int listId)
        {
            return await _mediator.Send(new GetTodoListByIdQuery { ListId = listId });
        }
        
        [HttpGet("todolist/{listId:int}/todo")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoListTodoItems(int listId, [FromQuery]GetTodoListTodoItemsQuery query)
        {
            query.ListId = listId;
            
            return await _mediator.Send(query);
        }

        [HttpGet("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemById(int listId, int todoId)
        {
            return await _mediator.Send(new GetTodoItemByIdQuery { ListId = listId, TodoId = todoId});
        }
        
        [HttpPost("todolist/{listId:int}/todo")]
        public async Task<ActionResult> CreateTodo(int listId, CreateTodoItemCommand command)
        {
            command.ListId = listId;
            
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTodoItemById), new { listId = command.ListId, todoId = result.Id }, result);
        }
        
        [HttpGet("todolist/{listId:int}/todo/completed")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> Completed(int listId, [FromQuery] GetCompletedTodoItemsQuery query)
        {
            return await _mediator.Send(query);
        }

        
    }
}