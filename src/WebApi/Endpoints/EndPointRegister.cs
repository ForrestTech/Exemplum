namespace Exemplum.WebApi.Endpoints;

public static class EndPointRegister
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endPointType = typeof(IEndpoints);

        var endpointRegisters = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => endPointType.IsAssignableFrom(type) &&
                           type.GetInterfaces().Contains(endPointType) &&
                           type is { IsClass: true, IsAbstract: false });

        foreach (Type endpointRegister in endpointRegisters)
        {
            IEndpoints? endpoints = (IEndpoints)Activator.CreateInstance(endpointRegister)!;

            if (endpoints is null)
            {
                throw new InvalidProgramException("An IEndpoint is registered that cannot be instantiated");
            }
            
            endpoints.MapEndpoints(app);
        }
    }
}

public interface IEndpoints
{
    void MapEndpoints(WebApplication app);
}