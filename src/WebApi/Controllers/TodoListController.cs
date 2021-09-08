using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.Todo.Queries;
    using Application.TodoList.Commands;
    using Application.TodoList.Models;
    using Application.TodoList.Queries;
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

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoListDto>>> Get([FromQuery] GetTodoListsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{listId:int}")]
        public async Task<ActionResult<TodoListDto>> GetById(int listId)
        {
            return await _mediator.Send(new GetTodoListQuery { ListId = listId });
        }

        [HttpGet("{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult<TodoListDto>> GetTodoById(int listId, int todoId)
        {
            return await _mediator.Send(new GetTodoListQuery() { ListId = listId });
        }

        [HttpPost("{listId:int}/todo")]
        public async Task<ActionResult> CreateTodo(int listId, CreateTodoItemCommand command)
        {
            command.ListId = listId;
            
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTodoById), new { listId = command.ListId, todoId = result.Id }, result);
        }
    }
}