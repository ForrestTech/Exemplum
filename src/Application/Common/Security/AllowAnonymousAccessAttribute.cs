namespace Exemplum.Application.Common.Security
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AllowAnonymousAccessAttribute : Attribute
    {

    }
}