namespace Exemplum.Application.Common.Security;

using Domain.Exceptions;
using Domain.Extensions;

public class ForbiddenAccessException : BusinessException, IExceptionWithSelfLogging
{
    private readonly Type _requestType;

    public ForbiddenAccessException(Type requestType) : base("Forbidden")
    {
        _requestType = requestType;
    }

    public ForbiddenAccessException(Type requestType, string details) : base("Forbidden", details: details)
    {
        _requestType = requestType;
    }

    public IEnumerable<string> Roles { get; init; } = new List<string>();

    public string Policy { get; init; } = string.Empty;    

    public void Log(ILogger logger)
    {
        if (Details.HasValue())
        {
            logger.Log(LogLevel,
                Details);
        }

        if (Policy.HasValue())
        {
            logger.Log(LogLevel,
                "User is not authorised to make a '{request}' request they did not comply with the {policy} policy",
                _requestType.Name, Policy);
        }

        if (Roles.Any())
        {
            logger.Log(LogLevel,
                "User is not authorised to make a '{request}' request they are not a member of one of the following roles '{roles}' ",
                _requestType.Name, string.Join(",", Roles));
        }
    }
}