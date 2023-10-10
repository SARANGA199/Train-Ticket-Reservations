//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20032838
//   Name  :  Devsrini Savidya A.S.

using EAD_Web_Services.Models.RequestAgentModel;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Services.RequestAgentService;
using Microsoft.AspNetCore.Mvc;

namespace EAD_Web_Services.Controllers.RequestAgentController
{
    /// <summary>
    /// Controller for managing request agent-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RequestAgentController : ControllerBase
    {
        private readonly IRequestAgentService requestAgentService;

        public RequestAgentController(IRequestAgentService requestAgentService)
        {
            this.requestAgentService = requestAgentService;
        }

        /// <summary>
        ///  Get a list of all request agents.
        /// </summary>
        /// <returns>A list of RequestAgent objects.</returns>
        [HttpGet]
        public ActionResult<List<RequestAgent>> Get()
        {
            return requestAgentService.Get();
        }

        /// <summary>
        /// Get a request agent by its ID.
        /// </summary>
        /// <param name="id">>The ID of the request agent to retrieve.</param>
        /// <returns>The RequestAgent object if found; otherwise, returns NotFound.</returns>
        [HttpGet("{id}")]
        public ActionResult<RequestAgent> Get(string id)
        {
            var requestAgent = requestAgentService.Get(id);

            if (requestAgent == null)
            {
                return NotFound($"Request not found "); ;
            }

            return requestAgent;

        }

        /// <summary>
        /// Create a new request agent.
        /// </summary>
        /// <param name="requestAgent">The RequestAgent object to create.</param>
        /// <returns>The created RequestAgent object.</returns>
        [HttpPost]
        public ActionResult<Reservation> Post([FromBody] RequestAgent requestAgent)
        {
            requestAgentService.Create(requestAgent);
            return CreatedAtAction(nameof(Get), new { id = requestAgent.Id }, requestAgent);
        }


        /// <summary>
        /// Delete a request agent by its ID.
        /// </summary>
        /// <param name="id">The ID of the request agent to delete.</param>
        /// <returns>method indicating the deletion result.</returns>
        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            var requestAgent = requestAgentService.Get(id);
            if (requestAgent == null)
            {
                NotFound($"Request  not found ");
            }

            var result = requestAgentService.Remove(id);

            return result;

        }
    }
}
