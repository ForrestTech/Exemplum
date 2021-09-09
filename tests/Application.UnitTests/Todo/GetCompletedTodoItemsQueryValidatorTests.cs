namespace Application.UnitTests.Todo
{
    using FluentValidation.TestHelper;
    using TodoList.Queries;
    using Xunit;

    public class GetCompletedTodoItemsQueryValidatorTests
    {
        /// <summary>
        /// Example fluent validation tests user there unit testing extensions
        /// </summary>
        [Fact]
        public void Should_have_validation_errors_for_page_number_zero()
        {
            var query = new GetCompletedTodoItemsQuery { PageNumber = 0, };
            
            var sut = new GetCompletedTodoItemsQueryValidator();
            
            var result = sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }
    }
}