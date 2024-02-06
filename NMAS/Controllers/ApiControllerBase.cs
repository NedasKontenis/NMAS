using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMAS.WebApi.Host.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {

    }
}
