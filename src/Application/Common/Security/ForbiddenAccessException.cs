namespace Exemplum.Application.Common.Security
{
    using Domain.Exceptions;

    public class ForbiddenAccessException : BusinessException
    {
        public ForbiddenAccessException() : base("Forbidden")
        {
        }
    }
}