using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.Todo.Models;
    using Application.Todo.Queries;
    using MediatR;

    public class TodoController : ApiControllerBase
    {
        private readonly ISender _meditor;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ISender meditor, ILogger<TodoController> logger)
        {
            _meditor = meditor;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> Get([FromQuery] GetTodoItemsQuery query)
        {
            return await _meditor.Send(query);
        }
        
        [HttpGet("completed")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> Completed([FromQuery] GetCompletedTodoItemsQuery query)
        {
            return await _meditor.Send(query);
        }
    }
}
