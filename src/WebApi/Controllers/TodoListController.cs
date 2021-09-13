using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.TodoList.Commands;
    using Application.TodoList.Models;
    using Application.TodoList.Queries;
    using MediatR;

    public class TodoListController : ApiControllerBase
    {
        private readonly ISender _mediator;

        public TodoListController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("todolist")]
        public async Task<ActionResult<PaginatedList<TodoListDto>>> GetTodoLists([FromQuery] GetTodoListsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("todolist")]
        public async Task<ActionResult<TodoListDto>> CreateTodoList([FromBody] CreateTodoListCommand command)
        {
            var item = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetTodoListById), new { listId = item.Id }, item);
        }

        [HttpDelete("todolist/{listId:int}")]
        public async Task<ActionResult> DeleteTodoList(int listId)
        {
            await _mediator.Send(new DeleteTodoListCommand { ListId = listId });
            return Ok();
        }

        [HttpGet("todolist/{listId:int}")]
        public async Task<ActionResult<TodoListDto>> GetTodoListById([FromRoute] int listId)
        {
            return await _mediator.Send(new GetTodoListByIdQuery { ListId = listId });
        }
    }
}