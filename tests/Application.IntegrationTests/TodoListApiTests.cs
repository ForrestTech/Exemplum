namespace Application.IntegrationTests
{
    using Common.Pagination;
    using FluentAssertions;
    using System.Linq;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using TodoList.Model;
    using WebApi;
    using Xunit;
    using Xunit.Abstractions;

    public class TodoListApiTests : ApiTestBase
    {
        public TodoListApiTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper) :
            base(
                factory, testOutputHelper)
        {
        }

        [Theory]
        [InlineData("api/todolist")]
        public async Task Get_returns_success_status_code(string url)
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public async Task Get_todolist_returns_paginated_list()
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync("api/todolist");

            var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoListDto>>();

            actual.Should().NotBeNull();
            actual?.Items.Should().NotBeNull();
            actual?.Items.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Get_by_id_should_return_single_list()
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync("api/todolist/1");

            var actual = await response.Content.ReadFromJsonAsync<TodoListDto>();

            actual?.Should().NotBeNull();
        }
    }
}