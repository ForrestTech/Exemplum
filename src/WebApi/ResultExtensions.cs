namespace Exemplum.WebApi;

using Application.Common.Validation;

public static class ResultExtensions
{
    public static CreatedAtRoute<TValue> ToCreatedAtRoute<TValue>(this TValue value, string routeName, object routeData) where TValue : new()
    {
        return TypedResults.CreatedAtRoute(value, routeName, routeData);
    }
    
    public static ValidationProblem ToValidationProblem(this ValidationFailed validationFailed)
    {
        return TypedResults.ValidationProblem(validationFailed.Errors);
    }
}