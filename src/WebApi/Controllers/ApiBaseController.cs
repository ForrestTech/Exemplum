namespace WebApi.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Route("api")]
    public abstract class ApiControllerBase : ControllerBase
    {
        
    }
}