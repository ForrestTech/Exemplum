namespace WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.Todo.Commands;
    using Application.Todo.Models;
    using Application.Todo.Queries;
    using Application.TodoList.Commands;
    using Application.TodoList.Models;
    using Application.TodoList.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class TodoController : ApiControllerBase
    {
        private readonly ISender _mediator;

        public TodoController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("todolist/{listId:int}/todo")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoItemsInList(int listId,
            [FromQuery] GetTodoItemsInListQuery query)
        {
            query.ListId = listId;

            return await _mediator.Send(query);
        }

        [HttpPost("todolist/{listId:int}/todo")]
        public async Task<ActionResult> CreateTodoItem(int listId, CreateTodoItemCommand command)
        {
            command.ListId = listId;

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTodoItemById), new { listId = command.ListId, todoId = result.Id },
                result);
        }

        [HttpGet("todolist/{listId:int}/todo/completed")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetCompletedTodoItems(int listId,
            [FromQuery] GetCompletedTodoItemsQuery query)
        {
            query.ListId = listId;
            return await _mediator.Send(query);
        }

        [HttpGet("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemById(int listId, int todoId)
        {
            return await _mediator.Send(new GetTodoItemByIdQuery { ListId = listId, TodoId = todoId });
        }

        [HttpPut("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult> UpdateTodoItem(int listId, int todoId, [FromBody] UpdateTodoCommand command)
        {
            command.ListId = listId;
            command.TodoId = todoId;

            await _mediator.Send(command);

            return Ok();
        }

        [HttpDelete("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult> DeleteTodoItem(int listId, int todoId)
        {
            await _mediator.Send(new DeleteTodoCommand { ListId = listId, TodoId = todoId });

            return Ok();
        }

        [HttpPost("todolist/{listId:int}/todo/{todoId:int}/completed")]
        public async Task<ActionResult> MarkTodoItemCompleted(int listId, int todoId)
        {
            await _mediator.Send(new MarkTodoCompleteCommand(listId, todoId));
            return Ok();
        }

        [HttpPost("todolist/{listId:int}/todo/{todoId:int}/priority")]
        public async Task<ActionResult> SetTodoItemPriorityLevel(int listId, int todoId, [FromBody] SetPriorityCommand command)
        {
            command.ListId = listId;
            command.TodoId = todoId;
            await _mediator.Send(command);
            return Ok();
        }
    }
}