using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;
using NMAS.WebApi.Contracts.Responses;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NMAS.WebApi.Host.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    public class AccommodationPlaceEntityController : ApiControllerBase
    {
        private readonly IAccommodationPlaceEntityService _accommodationPlaceEntityService;

        public AccommodationPlaceEntityController(IAccommodationPlaceEntityService accommodationPlaceEntityService)
        {
            _accommodationPlaceEntityService = accommodationPlaceEntityService;
        }

        /// <summary>
        /// Creates new Accommodation place entity
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AccommodationPlaceEntityCreated), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAccommodationPlaceEntity createAccommodationPlaceEntity)
        {
            var accommodationPlaceEntityCreated = await _accommodationPlaceEntityService.CreateAsync(createAccommodationPlaceEntity);

            return CreatedAtAction(
            nameof(Get),
            new
            {
                accommodationPlaceEntityCreated.Id
            },
            accommodationPlaceEntityCreated);
        }

        /// <summary>
        /// Return details of a single Accommodation place entity
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccommodationPlaceEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var accommodationPlaceEntity = await _accommodationPlaceEntityService.GetAsync(id);

            return Ok(accommodationPlaceEntity);
        }

        /// <summary>
        /// Updates Accommodation place entity
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAccommodationPlaceEntity updateAccommodationPlaceEntity)
        {
            await _accommodationPlaceEntityService.UpdateAsync(id, updateAccommodationPlaceEntity);

            return NoContent();
        }

        /// <summary>
        /// Deletes Accommodation place entity
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _accommodationPlaceEntityService.DeleteAsync(id);

            return NoContent();
        }
    }
}
