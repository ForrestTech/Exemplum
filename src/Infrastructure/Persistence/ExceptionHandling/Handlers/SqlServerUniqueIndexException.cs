namespace Exemplum.Infrastructure.Persistence.ExceptionHandling.Handlers
{
    using Application.Common.Validation;
    using Sprache;
    using System;
    using static SqlUniqueIndexExceptionGrammar;

    // public class SqlServerUniqueIndexException : IHandlerSpecificDBException
    // {
    //     private const int ErrorNumber = 2601;
    //
    //     public bool CanHandle(Exception exception)
    //     {
    //         return exception?.InnerException is SqlException { Number: ErrorNumber };
    //     }
    //
    //     public void HandleException(Exception exception)
    //     {
    //         if (exception?.InnerException is not SqlException sqlListException)
    //         {
    //             return;
    //         }
    //
    //         try
    //         {
    //             var (table, field, value) = ErrorMessage.Parse(sqlListException.Message);
    //
    //             throw new ValidationException(field,
    //                 $"Duplicate entry. An item already exists that has a '{field}' with the value of: '{value}'.");
    //         }
    //         catch (ParseException)
    //         {
    //             //its find to just swallow this here as the normal DB exception will bubble up and be handled
    //         }
    //     }
    // }
}