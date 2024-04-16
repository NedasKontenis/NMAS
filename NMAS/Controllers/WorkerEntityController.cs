using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMAS.WebApi.Contracts.Responses;
using NMAS.WebApi.Contracts.WorkerEntity;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NMAS.WebApi.Host.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    public class WorkerEntityController : ApiControllerBase
    {
        /// <summary>
        /// Creates new worker entity
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(WorkerEntityCreated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] WorkerEntity createWorkerEntity)
        {
            var WorkerEntityCreated = 1;
            return Ok(WorkerEntityCreated);
        }

        /// <summary>
        /// Return details of a single worker entity
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkerEntityCreated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var workerEntity = 1;

            return Ok(workerEntity);
        }

        /// <summary>
        /// Updates worker entity details
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(WorkerEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] WorkerEntity updateWorkerEntity)
        {
            var workerEntityUpdated = 1;
            return Ok(workerEntityUpdated);
        }

        /// <summary>
        /// Deletes worker entity
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(WorkerEntityUpdated), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var workerEntityDeleted = 1;
            return Ok(workerEntityDeleted);
        }
    }
}
