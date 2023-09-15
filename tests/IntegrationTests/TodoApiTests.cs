namespace Exemplum.IntegrationTests;

using Application.Common.Pagination;
using Application.Todo.Commands;
using Application.Todo.Models;
using Domain.Todo;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

[Collection("ExemplumApiTests")]
public class TodoApiTests
{
    private readonly ITestOutputHelper _output;

    public TodoApiTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Todo_get_returns_paginated_list_of_todos()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();

        var response = await client.GetAsync(Api.Items(1));

        var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoItemDto>>();

        actual.Should().NotBeNull();
        actual?.Items.Should().NotBeNull();
        actual?.Items?.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task Todo_get_completed_should_only_returns_completed_todos()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();

        var response = await client.GetAsync(Api.Completed(1));

        var actual = await response.Content.ReadFromJsonAsync<PaginatedList<TodoItemDto>>();

        actual?.Items.Should().NotBeEmpty().And.OnlyContain(x => x.Done);
    }

    [Fact]
    public async Task Todo_create_with_invalid_values_returns_error()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();

        var response = await client.PostAsJsonAsync(Api.Items(1),
            new CreateTodoItemCommand { Title = string.Empty, Note = "Some note" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        error.Should().NotBeNull();
        error?.Title.Should().Be("One or more validation errors occurred.");
        error?.Errors.Should().NotBeNull();
        error?.Errors?.Count.Should().Be(1);
        error?.Errors?.Should().ContainKey("Title");
    }

    [Fact]
    public async Task Todo_create_and_ensure_it_retrieved()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();

        const string todoTitle = "New todo";
        var response = await client.PostAsJsonAsync(Api.Items(1),
            new CreateTodoItemCommand { Title = todoTitle, Note = "Some note" });

        response.EnsureSuccessStatusCode();

        var newTodoResponse = await client.GetAsync(response.Headers.Location);

        newTodoResponse.EnsureSuccessStatusCode();

        var newTodo = await newTodoResponse.Content.ReadFromJsonAsync<TodoItemDto>();

        newTodo?.Title.Should().Be(todoTitle);
    }

    [Fact]
    public async Task Todo_update_and_ensure_its_updated()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();
        const string updatedTitle = "Updated todo";
        const string updatedNote = "updated note";

        var todo = await CreateNewTodo(client, "Will update");

        var response = await client.PutAsJsonAsync(todo.Headers.Location,
            new UpdateTodoCommand { Title = updatedTitle, Note = updatedNote });

        response.EnsureSuccessStatusCode();

        var newTodoResponse = await client.GetAsync(todo.Headers.Location);

        var newTodo = await newTodoResponse.Content.ReadFromJsonAsync<TodoItemDto>();

        newTodo?.Title.Should().Be(updatedTitle);
        newTodo?.Note.Should().Be(updatedNote);
    }

    [Fact]
    public async Task Todo_delete_item_and_ensure_its_removed()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();

        const string todoTitle = "To be deleted";

        var todo = await CreateNewTodo(client, todoTitle);

        await client.DeleteAsync(todo.Headers.Location);

        var newTodoResponse = await client.GetAsync(todo.Headers.Location);

        newTodoResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Todo_complete_item_and_ensure_state()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();

        const string todoTitle = "To be completed";
        var response = await CreateNewTodo(client, todoTitle);

        var completedResponse = await client.PostAsync($"{response.Headers.Location}/completed",
            new StringContent(string.Empty));

        completedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var completedTodo = await client.GetAsync(response.Headers.Location);

        completedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var newTodo = await completedTodo.Content.ReadFromJsonAsync<TodoItemDto>();

        newTodo?.Done.Should().Be(true);
    }

    [Fact]
    public async Task Todo_set_priority_and_ensure_state()
    {
        await using var application = new WebApi(_output);
        var client = application.CreateClient();

        var todo = await CreateNewTodo(client, "Will set priority");

        _output.WriteLine("Location " + todo.Headers.Location);

        var priorityLevel = PriorityLevel.High.ToString();
        var response = await client.PostAsJsonAsync($"{todo.Headers.Location}/priority",
            new SetPriorityCommand { PriorityLevel = priorityLevel });

        response.EnsureSuccessStatusCode();

        var updatedPriorityTodo = await client.GetAsync(todo.Headers.Location);

        updatedPriorityTodo.EnsureSuccessStatusCode();

        var newTodo = await updatedPriorityTodo.Content.ReadFromJsonAsync<TodoItemDto>();

        newTodo?.Priority.Should().Be(priorityLevel);
    }

    private static async Task<HttpResponseMessage> CreateNewTodo(HttpClient client, string todoTitle)
    {
        var response = await client.PostAsJsonAsync(Api.Items(1),
            new CreateTodoItemCommand { Title = todoTitle, Note = "Some note" });
        return response;
    }
}