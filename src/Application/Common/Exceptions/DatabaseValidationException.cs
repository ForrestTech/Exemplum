namespace Exemplum.Application.Common.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class DatabaseValidationException : Exception
    {
        public string Field { get; }

        public Dictionary<string, string[]> Errors
        {
            get
            {
                return new Dictionary<string, string[]> { { Field, new[] { Message } } };
            }
        }

        public DatabaseValidationException(string field, string message, Exception innerException) : base(message,
            innerException: innerException)
        {
            Field = field;
        }
    }
}