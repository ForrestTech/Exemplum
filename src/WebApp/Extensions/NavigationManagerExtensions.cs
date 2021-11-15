namespace Exemplum.WebApp.Extensions;

using Microsoft.AspNetCore.Components;

public static class NavigationManagerExtensions
{
    /// <summary>
    /// Determines if the current page is the base page
    /// </summary>
    public static bool IsHomePage(this NavigationManager navMan)
    {
        return navMan.Uri == navMan.BaseUri;
    }
}