namespace Application.UnitTests.Domain.Todo
{
    using Exemplum.Application.Todo.Queries;
    using FluentValidation.TestHelper;
    using Xunit;

    public class GetCompletedTodoItemsQueryValidatorTests
    {
        /// <summary>
        /// Example fluent validation tests user there unit testing extensions
        /// </summary>
        [Fact]
        public void Should_have_validation_errors_for_page_number_zero()
        {
            var query = new GetCompletedTodoItemsQuery { PageNumber = 0, PageSize = 0};
            
            var sut = new GetCompletedTodoItemsQueryValidator();
            
            var result = sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
        
        [Fact]
        public void Should_have_no_validation_errors_for_page_number_zero()
        {
            var query = new GetCompletedTodoItemsQuery { PageNumber = 1, PageSize = 1};
            
            var sut = new GetCompletedTodoItemsQueryValidator();
            
            var result = sut.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.PageNumber);
            result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
        }
    }
}