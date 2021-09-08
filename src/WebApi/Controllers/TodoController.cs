using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.Todo.Model;
    using Application.Todo.Queries;
    using MediatR;

    public class TodoController : ApiControllerBase
    {
        private readonly ILogger<TodoController> _logger;

        public TodoController(ILogger<TodoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> Get([FromQuery] GetTodoItemsQuery query)
        {
            return await Mediator.Send(query);
        }
        
        [HttpGet("completed")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> Completed([FromQuery] GetCompletedTodoItemsQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
