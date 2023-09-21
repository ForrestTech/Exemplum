namespace Exemplum.Application.Common.Security;

public class Denied
{
    public Denied(string reason) => this.Reason = reason;

    public string Reason { get; }
}
