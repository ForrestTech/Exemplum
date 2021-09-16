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

        public DatabaseValidationException(string field, string message) : base(message)
        {
            Field = field;
        }
    }
}