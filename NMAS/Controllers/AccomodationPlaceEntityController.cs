using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMAS.WebApi.Client;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;
using Ondato.Infrastructure.WebApi.Contracts.Responses;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NMAS.WebApi.Host.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    public class AccomodationPlaceEntityController : ApiControllerBase
    {
        /// <summary>
        /// Creates new accomodation place entity
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AccomodationPlaceEntityCreated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AccomodationPlaceEntity createAccomodationPlaceEntity)
        {
            var AccomodationPlaceEntityCreated = 1;
            return Ok(AccomodationPlaceEntityCreated);
        }

        /// <summary>
        /// Return details of a single accomodation place entity
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccomodationPlaceEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var accomodationPlaceEntity = 1;
            return Ok(accomodationPlaceEntity);
        }

        /// <summary>
        /// Updates accomodation place entity
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccomodationPlaceEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] AccomodationPlaceEntity updateAccomodationPlaceEntity)
        {
            var accomodationPlaceEntityUpdated = 1;
            return Ok(accomodationPlaceEntityUpdated);
        }

        /// <summary>
        /// Deletes accomodation place entity
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AccomodationPlaceEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var accomodationPlaceEntityDeleted = 1;
            return Ok(accomodationPlaceEntityDeleted);
        }
    }
}
