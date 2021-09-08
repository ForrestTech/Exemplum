namespace Application.IntegrationTests
{
    using Common.Pagination;
    using FluentAssertions;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Todo.Model;
    using TodoList.Model;
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
        [InlineData("api/todo")]
        [InlineData("api/todolist")]
        public async Task Get_returns_success_status_code(string url)
        {
            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public async Task Todo_get_returns_paginated_list_of_todos()
        {
            var response = await _client.GetAsync("api/todo");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoItemDto>>();

            actual.Should().NotBeNull();
            actual?.Items.Should().NotBeNull();
            actual?.Items.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task Todo_get_completed_should_only_returns_completed_todos()
        {
            var response = await _client.GetAsync("api/todo/completed");

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
            actual?.Items.Count.Should().Be(1);
        }

        [Fact]
        public async Task Todolist_get_by_id_should_return_single_list()
        {
            var response = await _client.GetAsync("api/todolist/1");

            var actual = await response.Content.ReadFromJsonAsync<TodoListDto>();

            actual?.Should().NotBeNull();
        }
    }
}