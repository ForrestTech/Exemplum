namespace Exemplum.Application.Common.Security;

public static class Security
{
    public static class Roles
    {
        public const string Forecaster = nameof(Forecaster);
    }

    public static class Policy
    {
        public const string CanWriteTodo = nameof(CanWriteTodo);
        public const string CanDeleteTodo = nameof(CanDeleteTodo);
    }

    public static class ClaimTypes
    {
        public const string Permission = "permissions";
    }

    public static class Permissions
    {
        public const string WriteTodo = "write:todo";
        public const string DeleteTodo = "delete:todo";
    }
}