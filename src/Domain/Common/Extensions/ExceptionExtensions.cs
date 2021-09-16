namespace Exemplum.Domain.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ExceptionExtensions
    {
        public static string GetAllMessages(this Exception exception)
        {
            var message = exception.Message;
            var innerExceptionMessages = string.Join($"InnerException: ", exception.GetInnerExceptions().Select(x => x.Message));
            return $"{message}  {innerExceptionMessages}";
        }

        private static IEnumerable<Exception> GetInnerExceptions(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var innerException = exception.InnerException;

            while (innerException != null)
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
        }
    }
}