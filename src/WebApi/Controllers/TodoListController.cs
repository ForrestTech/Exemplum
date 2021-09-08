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
    using Application.TodoList.Model;
    using Application.TodoList.Queries;
    using MediatR;

    public class TodoListController : ApiControllerBase
    {
        private readonly ILogger<TodoListController> _logger;

        public TodoListController(ILogger<TodoListController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoListDto>>> Get([FromQuery] GetTodoListsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListDto>> GetById(int id)
        {
            return await Mediator.Send(new GetTodoListQuery() { Id = id });
        }
    }
}