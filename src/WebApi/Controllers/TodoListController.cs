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

        [HttpGet("todolist/{listId:int}")]
        public async Task<ActionResult<TodoListDto>> GetTodoListById([FromRoute]int listId)
        {
            return await _mediator.Send(new GetTodoListByIdQuery { ListId = listId });
        }
        
        [HttpGet("todolist/{listId:int}/todo")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoListTodoItems(int listId, [FromQuery]GetTodoListTodoItemsQuery query)
        {
            query.ListId = listId;
            
            return await _mediator.Send(query);
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
            query.ListId = listId;
            return await _mediator.Send(query);
        }
        
        [HttpGet("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemById(int listId, int todoId)
        {
            return await _mediator.Send(new GetTodoItemByIdQuery { ListId = listId, TodoId = todoId});
        }

        [HttpPost("todolist/{listId:int}/todo/{todoId:int}/completed")]
        public async Task<ActionResult> GetTodoListTodoItems(int listId, int todoId)
        {
            await _mediator.Send(new MarkTodoCompleteCommand(listId,todoId));
            return Ok();
        }
        
        [HttpPost("todolist/{listId:int}/todo/{todoId:int}/priority")]
        public async Task<ActionResult> SetPriorityLevel(int listId, int todoId,  [FromBody]SetPriorityCommand command)
        {
            command.ListId = listId; 
            command.TodoId = todoId;
            await _mediator.Send(command);
            return Ok();
        }
    }
}