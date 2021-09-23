namespace Exemplum.WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.TodoList.Commands;
    using Application.TodoList.Models;
    using Application.TodoList.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
    using System.Threading.Tasks;

    [Authorize]           
    public class TodoListController : ApiControllerBase
    {
        private readonly ISender _mediator;

        public TodoListController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get todo lists
        /// </summary>
        /// <returns>A paginated list of todo lists</returns>
        /// <param name="pageSize">The size of a page</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpGet("todolist")]
        public async Task<ActionResult<PaginatedList<TodoListDto>>> GetTodoLists(int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetTodoListsQuery { PageNumber = pageNumber, PageSize = pageSize },
                cancellationToken);
        }

        /// <summary>
        /// Create todo list
        /// </summary>
        /// <param name="command">The list to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created todo list</returns>
        [HttpPost("todolist")]
        public async Task<ActionResult<TodoListDto>> CreateTodoList(CreateTodoListCommand command,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetTodoListById), new { listId = item.Id }, item);
        }

        /// <summary>
        /// Update the todo list
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="command">The details to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated list</returns>
        [HttpPut("todolist/{listId:int}")]
        public async Task<ActionResult<TodoListDto>> UpdateTodoList(int listId,
            UpdateTodoListCommand command,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            command.ListId = listId;

            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Get todo list by ID
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpGet("todolist/{listId:int}")]
        public async Task<ActionResult<TodoListDto>> GetTodoListById(int listId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _mediator.Send(new GetTodoListByIdQuery { ListId = listId }, cancellationToken);
        }

        /// <summary>
        /// Delete the todo list
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("todolist/{listId:int}")]
        public async Task<ActionResult> DeleteTodoList(int listId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.Send(new DeleteTodoListCommand { ListId = listId }, cancellationToken);
            return Ok();
        }
    }
}