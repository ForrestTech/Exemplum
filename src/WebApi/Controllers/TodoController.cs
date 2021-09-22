namespace Exemplum.WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.Todo.Commands;
    using Application.Todo.Models;
    using Application.Todo.Queries;
    using Domain.Todo;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
    using System.Threading.Tasks;

    public class TodoController : ApiControllerBase
    {
        private readonly ISender _mediator;

        public TodoController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get todo items in a given list
        /// </summary>
        /// <returns>A paginated list of TodoItems in a list</returns>
        /// <param name="listId">The id of the list</param>
        /// <param name="pageSize">The size of a page</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpGet("todolist/{listId:int}/todo")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoItemsInList(int listId,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetTodoItemsInListQuery
            {
                ListId = listId, PageNumber = pageNumber, PageSize = pageSize
            }, cancellationToken);
        }

        /// <summary>
        /// Create todo item in a list
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="command">The TodoItem to be created</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost("todolist/{listId:int}/todo")]
        public async Task<ActionResult> CreateTodoItem(int listId, 
            CreateTodoItemCommand command,
            CancellationToken cancellationToken = default)
        {
            command.ListId = listId;

            var result = await _mediator.Send(command, cancellationToken);
            
            return CreatedAtAction(nameof(GetTodoItemById), new { listId = command.ListId, todoId = result.Id },
                result);
        }

        /// <summary>
        /// Get completed todo items in a list
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="pageSize">The size of a page</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A paginated list of todo items</returns>
        [HttpGet("todolist/{listId:int}/todo/completed")]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetCompletedTodoItems(int listId,
            int pageNumber = 1, 
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetCompletedTodoItemsQuery
            {
                ListId = listId, PageNumber = pageNumber, PageSize = pageSize
            }, cancellationToken);
        }

        /// <summary>
        /// Get todo items by ID
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="todoId">The todo item id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The todo item</returns>
        [HttpGet("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemById(int listId,
            int todoId,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetTodoItemByIdQuery { ListId = listId, TodoId = todoId }, cancellationToken);
        }

        /// <summary>
        /// Update the todo item
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="todoId">The todo id</param>
        /// <param name="command">The todo details to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPut("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult<TodoItem>> UpdateTodoItem(int listId, 
            int todoId, 
            UpdateTodoCommand command,
            CancellationToken cancellationToken = default)
        {
            command.ListId = listId;
            command.TodoId = todoId;

            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Delete todo item
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="todoId">The id of the todo item</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("todolist/{listId:int}/todo/{todoId:int}")]
        public async Task<ActionResult> DeleteTodoItem(int listId, 
            int todoId,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteTodoCommand { ListId = listId, TodoId = todoId }, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Mark todo item as complete
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="todoId">The id of the todo item</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("todolist/{listId:int}/todo/{todoId:int}/completed")]
        public async Task<ActionResult> MarkTodoItemCompleted(int listId, 
            int todoId,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new MarkTodoCompleteCommand(listId, todoId), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Set the priority of the todo item
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="todoId">The id of the todo item</param>
        /// <param name="command">The priority to set</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("todolist/{listId:int}/todo/{todoId:int}/priority")]
        public async Task<ActionResult> SetTodoItemPriorityLevel(int listId,
            int todoId,
            SetPriorityCommand command,
            CancellationToken cancellationToken = default)
        {
            command.ListId = listId;
            command.TodoId = todoId;
            
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}