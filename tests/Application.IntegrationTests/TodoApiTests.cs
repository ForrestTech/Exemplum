namespace Application.IntegrationTests
{
    using Common.Pagination;
    using FluentAssertions;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using TodoList.Commands;
    using TodoList.Models;
    using TodoList.Queries;
    using WebApi;
    using Xunit;
    using Xunit.Abstractions;

    public class TodoApiTests : IClassFixture<WebHostFixture>
    {
        private readonly WebHostFixture _fixture;
        private readonly HttpClient _client;

        public TodoApiTests(WebHostFixture fixture, ITestOutputHelper output)

        {
            _fixture = fixture;
            _fixture.Output = output;
            _client = fixture.CreateClient();
        }

        [Theory]
        [InlineData("api/todolist")]
        [InlineData("api/todolist/1/todo")]
        [InlineData("api/todolist/1/todo/1")]
        [InlineData("api/todolist/1/todo/completed")]
        [InlineData("api/todolist/999", 404)]
        [InlineData("api/todolist/1/todo/999", 404)]
        public async Task Routes_table_tests(string url, int statusCode = 200)
        {
            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be((HttpStatusCode)statusCode);

            bool successfulResponse = statusCode is >= 200 and <= 299;
            
            response.Content.Headers.ContentType?.ToString()
                .Should().Be(successfulResponse ? 
                    "application/json; charset=utf-8" : 
                    "application/problem+json; charset=utf-8");
        }

        [Fact]
        public async Task Todo_get_returns_paginated_list_of_todos()
        {
            var response = await _client.GetAsync("api/todolist/1/todo");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoItemDto>>();

            actual.Should().NotBeNull();
            actual?.Items.Should().NotBeNull();
            actual?.Items.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task Todo_get_completed_should_only_returns_completed_todos()
        {
            var response = await _client.GetAsync("api/todolist/1/todo/completed");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoItemDto>>();

            actual?.Items.Should().NotBeEmpty().And.OnlyContain(x => x.Done);
        }

        [Fact]
        public async Task Todolist_get_returns_paginated_list()
        {
            var response = await _client.GetAsync("api/todolist");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoListDto>>();

            actual.Should().NotBeNull();
            actual?.Items.Should().NotBeNull();
            actual?.Items?.Count.Should().Be(1);
        }

        [Fact]
        public async Task Todolist_get_by_id_should_return_single_list()
        {
            var response = await _client.GetAsync("api/todolist/1");

            var actual = await response.Content.ReadFromJsonAsync<TodoListDto>();

            actual?.Should().NotBeNull();
        }
        
        [Fact]
        public async Task Todolist_post()
        {
            const string todoTitle = "New todo";
            
            var response = await _client.PostAsJsonAsync("api/todolist/1/todo", new CreateTodoItemCommand
            {
                Title = todoTitle,
                Note = "Some note"
            });

            response.EnsureSuccessStatusCode();
            
            var newTodoResponse = await _client.GetAsync(response.Headers.Location);

            newTodoResponse.EnsureSuccessStatusCode();
            
            var newTodo = await response.Content.ReadFromJsonAsync<TodoItemDto>();

            newTodo?.Title.Should().Be(todoTitle);
        }
    }
}