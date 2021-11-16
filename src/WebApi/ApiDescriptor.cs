namespace Exemplum.WebApi;

using Microsoft.AspNetCore.Mvc.ApiExplorer;

public static class ApiDescriptor
{
    public static IList<string> GetGroupName(ApiDescription apiDescription)
    {
        var relativePath = apiDescription.RelativePath;
        if (relativePath == null)
        {
            return new[] {apiDescription.ActionDescriptor?.DisplayName ?? string.Empty};
        }

        var removeApi = relativePath.Substring(4, relativePath.Length - 4);

        var group = removeApi;
        if (removeApi.Contains('/'))
        {
            @group = removeApi.Substring(0, removeApi.IndexOf('/'));
        }

        return new[] {@group};
    }
}