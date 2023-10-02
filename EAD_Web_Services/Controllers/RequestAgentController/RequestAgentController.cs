using EAD_Web_Services.Models.RequestAgentModel;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Services.RequestAgentService;
using Microsoft.AspNetCore.Mvc;

namespace EAD_Web_Services.Controllers.RequestAgentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestAgentController : ControllerBase
    {
        private readonly IRequestAgentService requestAgentService;

        public RequestAgentController(IRequestAgentService requestAgentService)
        {
            this.requestAgentService = requestAgentService;
        }

        // GET: api/<RequestAgentController>
        [HttpGet]
        public ActionResult<List<RequestAgent>> Get()
        {
            return requestAgentService.Get();
        }

        // GET api/<RequestAgentController>/5
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

        // POST api/<RequestAgentController>
        [HttpPost]
        public ActionResult<Reservation> Post([FromBody] RequestAgent requestAgent)
        {
            requestAgentService.Create(requestAgent);
            return CreatedAtAction(nameof(Get), new { id = requestAgent.Id }, requestAgent);
        }


        // DELETE api/<RequestAgentController>/5
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
