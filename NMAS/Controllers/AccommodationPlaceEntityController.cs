using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMAS.WebApi.Client;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.Response;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NMAS.WebApi.Host.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    public class AccommodationPlaceEntityController : ApiControllerBase
    {
        /// <summary>
        /// Creates new Accommodation place entity
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AccommodationPlaceEntityCreated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AccommodationPlaceEntity createAccommodationPlaceEntity)
        {
            var accommodationPlaceEntityCreated = 1;
            return Ok(accommodationPlaceEntityCreated);
        }

        /// <summary>
        /// Return details of a single Accommodation place entity
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccommodationPlaceEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var accommodationPlaceEntity = 1;
            return Ok(accommodationPlaceEntity);
        }

        /// <summary>
        /// Updates Accommodation place entity
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccommodationPlaceEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] AccommodationPlaceEntity updateAccommodationPlaceEntity)
        {
            var accommodationPlaceEntityUpdated = 1;
            return Ok(accommodationPlaceEntityUpdated);
        }

        /// <summary>
        /// Deletes Accommodation place entity
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AccommodationPlaceEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var accommodationPlaceEntityDeleted = 1;
            return Ok(accommodationPlaceEntityDeleted);
        }
    }
}
