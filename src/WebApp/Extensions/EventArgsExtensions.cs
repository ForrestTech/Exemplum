namespace Exemplum.WebApp.Extensions;

using Microsoft.AspNetCore.Components.Web;

public static class EventArgsExtensions
{
    public static bool IsEnter(this KeyboardEventArgs args)
    {
        return args.Code is "Enter" or "NumpadEnter";
    }
}