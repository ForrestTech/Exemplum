namespace Exemplum.Application.Common.Security
{
    using Domain.Extensions;
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public bool HasAuthenticationRequirements
        {
            get
            {
                return Roles.HasValue() || Policy.HasValue();
            }
        }

        public string Roles { get; set; } = string.Empty;

        public string Policy { get; set; } = string.Empty;
    }
}