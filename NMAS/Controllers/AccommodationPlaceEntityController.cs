using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [ProducesResponseType(typeof(AccommodationPlaceEntityCreated), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
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
        [Authorize]
        [ProducesResponseType(typeof(AccommodationPlaceEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _accommodationPlaceEntityService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Returns a list of accommodation place entities
        /// </summary>
        [HttpPost("filter")]
        [Authorize]
        [ProducesResponseType(typeof(AccommodationPlaceEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> List([FromBody] FilterAccommodationPlaceEntity filter, [FromQuery] FilterAccommodationPlaceEntityPagination pagination)
        {
            var accommodationPlaceEntityFilter = new FilterAccommodationPlaceEntity
            {
                Ids = filter.Ids,
                WorkerIds = filter.WorkerIds,
                PlaceNames = filter.PlaceNames,
                Adresses = filter.Adresses,
                CompanyCodes = filter.CompanyCodes
            };

            var accommodationPlaceEntities = await _accommodationPlaceEntityService.ListAsync(accommodationPlaceEntityFilter, pagination);

            return Ok(accommodationPlaceEntities);
        }
    }
}
