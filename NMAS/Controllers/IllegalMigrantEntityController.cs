using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Contracts.Responses;
using NMAS.WebApi.Services.IllegalMigrantEntity;
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
        [ProducesResponseType(typeof(IllegalMigrantEntityCreated), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateIllegalMigrantEntity createIllegalMigrantEntity)
        {
            var illegalMigrantEntityCreated = await _illegalMigrantEntityService.CreateAsync(createIllegalMigrantEntity);

            return CreatedAtAction(
            nameof(Get),
            new
            {
                illegalMigrantEntityCreated.Id
            },
            illegalMigrantEntityCreated);
        }

        /// <summary>
        /// Return details of a single illegal migrant entity
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IllegalMigrantEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var illegalMigrantEntity = await _illegalMigrantEntityService.GetAsync(id);

            return Ok(illegalMigrantEntity);
        }

        /// <summary>
        /// Updates illegal migrant entity
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIllegalMigrantEntity updateIllegalMigrantEntity)
        {
            await _illegalMigrantEntityService.UpdateAsync(id, updateIllegalMigrantEntity);

            return NoContent();
        }

        /// <summary>
        /// Deletes illegal migrant entity
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _illegalMigrantEntityService.DeleteAsync(id);

            return NoContent();
        }
    }
}
