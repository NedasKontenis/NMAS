using Microsoft.AspNetCore.Mvc;

namespace NMAS.WebApi.Host.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {

    }
}
