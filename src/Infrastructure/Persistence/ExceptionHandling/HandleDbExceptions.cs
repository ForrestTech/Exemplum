namespace Infrastructure.Persistence.ExceptionHandling
{
    using Handlers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Database exceptions can be cryptic and its not ideal to show them to the end user.  We map given exceptions to a more users friendly DatabaseException that can be shown to an end user
    /// </summary>
    public class HandleDbExceptions : IHandleDbExceptions
    {
        private readonly List<IHandlerSpecificDBException> _handlers;

        public HandleDbExceptions()
        {
            _handlers = new List<IHandlerSpecificDBException> { new SqlServerUniqueIndexException() };
        }

        public void HandleException(Exception exception)
        {
            var handler = _handlers.SingleOrDefault(x => x.CanHandle(exception));

            if (handler == null)
                return;
        }
    }
}