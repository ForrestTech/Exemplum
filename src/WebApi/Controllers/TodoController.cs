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
    using MediatR;

    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TodoController> _logger;

        public TodoController(IMediator mediator, ILogger<TodoController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("todo")]
        public Task<PaginatedList<TodoItemDto>> Get([FromQuery] GetTodoItemsQuery query)
        {
            return _mediator.Send(query);
        }
        
        [HttpGet("todo/completed")]
        public Task<PaginatedList<TodoItemDto>> Completed([FromQuery] GetCompletedTodoItemsQuery query)
        {
            return _mediator.Send(query);
        }
    }
}
