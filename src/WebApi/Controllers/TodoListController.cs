namespace Exemplum.WebApi.Controllers
{
    using Application.Common.Pagination;
    using Application.TodoList.Commands;
    using Application.TodoList.Models;
    using Application.TodoList.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

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
        [HttpGet("todolist")]
        public async Task<ActionResult<PaginatedList<TodoListDto>>> GetTodoLists([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            return await _mediator.Send(new GetTodoListsQuery { PageNumber = pageNumber, PageSize = pageSize });
        }

        /// <summary>
        /// Create todo list
        /// </summary>
        /// <param name="command">The list to create</param>
        /// <returns>The created todo list</returns>
        [HttpPost("todolist")]
        public async Task<ActionResult<TodoListDto>> CreateTodoList([FromBody] CreateTodoListCommand command)
        {
            var item = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetTodoListById), new { listId = item.Id }, item);
        }
        
        /// <summary>
        /// Update the todo list
        /// </summary>
        /// <param name="listId">The id of the list</param>
        /// <param name="command">The details to update</param>
        /// <returns>The updated list</returns>
        [HttpPut("todolist/{listId:int}")]
        public async Task<ActionResult<TodoListDto>> UpdateTodoList(int listId, [FromBody] UpdateTodoListCommand command)
        {
            command.ListId = listId;
            
            return await _mediator.Send(command);
        }
        
        /// <summary>
        /// Get todo list by ID
        /// </summary>
        /// <param name="listId">The id of the list</param>
        [HttpGet("todolist/{listId:int}")]
        public async Task<ActionResult<TodoListDto>> GetTodoListById([FromRoute] int listId)
        {
            return await _mediator.Send(new GetTodoListByIdQuery { ListId = listId });
        }

        /// <summary>
        /// Delete the todo list
        /// </summary>
        /// <param name="listId">The id of the list</param>
        [HttpDelete("todolist/{listId:int}")]
        public async Task<ActionResult> DeleteTodoList(int listId)
        {
            await _mediator.Send(new DeleteTodoListCommand { ListId = listId });
            return Ok();
        }
    }
}