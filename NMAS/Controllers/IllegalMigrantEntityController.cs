using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMAS.WebApi.Client;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Services.IllegalMigrantEntity;
using Ondato.Infrastructure.WebApi.Contracts.Responses;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NMAS.WebApi.Host.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    public class IllegalMigrantEntityController : ApiControllerBase
    {
        private readonly IIllegalMigrantEntityService _illegalMigrantEntityService;

        public IllegalMigrantEntityController(IIllegalMigrantEntityService illegalMigrantEntityService)
        {
            _illegalMigrantEntityService = illegalMigrantEntityService;
        }

        /// <summary>
        /// Creates new illegal migrant entity
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(IllegalMigrantEntityCreated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] IllegalMigrantEntity createIllegalMigrantEntity)
        {
            var illegalMigrantEntityCreated = await _illegalMigrantEntityService.InsertAsync(createIllegalMigrantEntity);
            return Ok(illegalMigrantEntityCreated);
        }

        /// <summary>
        /// Return details of a single illegal migrant entity
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IllegalMigrantEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var illegalMigrantEntity = 1;
            return Ok(illegalMigrantEntity);
        }

        /// <summary>
        /// Updates illegal migrant entity
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IllegalMigrantEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] IllegalMigrantEntity updateIllegalMigrantEntity)
        {
            //var illegalMigrantEntityUpdated = await _illegalMigrantEntityService.InsertAsync(id, updateIllegalMigrantEntity);
            var illegalMigrantEntityUpdated=1;
            return Ok(illegalMigrantEntityUpdated);
        }

        /// <summary>
        /// Deletes illegal migrant entity
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(IllegalMigrantEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            //var illegalMigrantEntityUpdated = await _illegalMigrantEntityService.InsertAsync(id, updateIllegalMigrantEntity);
            var illegalMigrantEntityDeleted = 1;
            return Ok(illegalMigrantEntityDeleted);
        }
    }
}
