namespace Application.IntegrationTests
{
    using Exemplum.Application.Common.Pagination;
    using Exemplum.Application.TodoList.Commands;
    using Exemplum.Application.TodoList.Models;
    using Exemplum.Domain.Todo;
    using FluentAssertions;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Created as a partial to separate the tests but it allows us to use a single IClassFixture setup and tear down across all API integration tests
    /// </summary>
    public partial class ExemplumApiTests : IClassFixture<WebHostFixture>
    {
        private readonly WebHostFixture _fixture;
        private readonly HttpClient _client;

        public ExemplumApiTests(WebHostFixture fixture, ITestOutputHelper output)

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
                .Should().Be(successfulResponse
                    ? "application/json; charset=utf-8"
                    : "application/problem+json; charset=utf-8");
        }

        [Fact]
        public async Task Todolist_get_returns_paginated_list()
        {
            var response = await _client.GetAsync("api/todolist");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoListDto>>();

            actual.Should().NotBeNull();
            actual?.Items.Should().NotBeNull();
            actual?.Items?.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Todolist_get_by_id_should_return_single_list()
        {
            var response = await _client.GetAsync("api/todolist/1");

            var actual = await response.Content.ReadFromJsonAsync<TodoListDto>();

            actual?.Should().NotBeNull();
        }

        [Fact]
        public async Task Todolist_delete_and_ensure_its_removed()
        {
            const string todoTitle = "To be deleted";

            var response = await _client.PostAsJsonAsync("api/todolist",
                new CreateTodoListCommand { Title = todoTitle, Colour = Colour.White });

            await _client.DeleteAsync(response.Headers.Location);

            var newTodoResponse = await _client.GetAsync(response.Headers.Location);

            newTodoResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Todolist_create_and_ensure_it_retrieved()
        {
            const string todoTitle = "New todo";

            var response = await _client.PostAsJsonAsync("api/todolist",
                new CreateTodoListCommand { Title = todoTitle, Colour = Colour.White });

            response.EnsureSuccessStatusCode();

            var newTodoResponse = await _client.GetAsync(response.Headers.Location);

            newTodoResponse.EnsureSuccessStatusCode();

            var newList = await newTodoResponse.Content.ReadFromJsonAsync<TodoListDto>();

            newList?.Title.Should().Be(todoTitle);
            newList?.Colour.Should().Be(Colour.White);
        }
    }
}