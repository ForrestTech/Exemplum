namespace Exemplum.Application.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AllowAnonymousAccessAttribute : Attribute
{
}