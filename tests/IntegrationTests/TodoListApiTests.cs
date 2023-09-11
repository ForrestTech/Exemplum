namespace Exemplum.IntegrationTests;

using Application.Common.Pagination;
using Application.TodoList.Commands;
using Application.TodoList.Models;
using Domain.Todo;

[Collection("ExemplumApiTests")]
public class TodoListTests
{
    private readonly ITestOutputHelper _output;

    public TodoListTests(ITestOutputHelper output)
    {
        _output = output;
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
        await using var application = new TodoAPI(_output);
        var client = application.CreateClient();

        var response = await client.GetAsync(url);
        
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        response.StatusCode.Should().Be((HttpStatusCode)statusCode);

        bool successfulResponse = statusCode is >= 200 and <= 299;

        response.Content.Headers.ContentType?.ToString()
            .Should().Be(successfulResponse
                ? "application/json; charset=utf-8"
                : "application/problem+json");
    }

    [Fact]
    public async Task Todolist_get_returns_paginated_list()
    {
        await using var application = new TodoAPI(_output);
        var client = application.CreateClient();

        var response = await client.GetAsync("api/todolist");

        var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoListDto>>();

        actual.Should().NotBeNull();
        actual?.Items.Should().NotBeNull();
        actual?.Items?.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Todolist_get_by_id_should_return_single_list()
    {
        await using var application = new TodoAPI(_output);
        var client = application.CreateClient();

        var response = await client.GetAsync("api/todolist/1");

        var actual = await response.Content.ReadFromJsonAsync<TodoListDto>();

        actual?.Should().NotBeNull();
    }

    [Fact]
    public async Task Todolist_delete_and_ensure_its_removed()
    {
        await using var application = new TodoAPI(_output);
        var client = application.CreateClient();

        const string todoTitle = "To be deleted";
        var response = await client.PostAsJsonAsync("api/todolist",
            new CreateTodoListCommand {Title = todoTitle, Colour = Colour.Blue});

        await client.DeleteAsync(response.Headers.Location);

        var newTodoResponse = await client.GetAsync(response.Headers.Location);
        
        _output.WriteLine($"Getting {response.Headers.Location}");

        newTodoResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Todolist_create_and_ensure_it_retrieved()
    {
        await using var application = new TodoAPI(_output);
        var client = application.CreateClient();
        
        const string todoListTitle = "New todo";
        var response = await client.PostAsJsonAsync("api/todolist",
            new CreateTodoListCommand {Title = todoListTitle, Colour = Colour.Blue});

        response.EnsureSuccessStatusCode();

        var newTodoResponse = await client.GetAsync(response.Headers.Location);

        newTodoResponse.EnsureSuccessStatusCode();

        var newList = await newTodoResponse.Content.ReadFromJsonAsync<TodoListDto>();

        newList?.Title.Should().Be(todoListTitle);
        newList?.Colour.Should().Be(Colour.Blue);
    }
}