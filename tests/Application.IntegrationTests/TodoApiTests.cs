namespace Application.IntegrationTests
{
    using Common.Pagination;
    using FluentAssertions;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Todo.Model;
    using WebApi;
    using Xunit;
    using Xunit.Abstractions;

    public class TodoApiTests : ApiTestBase
    {
        public TodoApiTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper) : base(
            factory, testOutputHelper)
        {
        }

        [Theory]
        [InlineData("api/todo")]
        public async Task Get_returns_success_status_code(string url)
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public async Task Get_todo_returns_paginated_list_of_todos()
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync("api/todo");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoItemDto>>();

            actual.Should().NotBeNull();
            actual?.Items.Should().NotBeNull();
            actual?.Items.Count.Should().BeGreaterThan(1);
        }
        
        [Fact]
        public async Task Get_completed_should_only_returns_completed_todos()
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync("api/todo/completed");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoItemDto>>();

            actual?.Items.Should().NotBeEmpty().And.OnlyContain(x => x.Done);
        }
    }
}