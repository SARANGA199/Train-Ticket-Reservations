//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260910
//   Name  :  Vishwa J.W.P

using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.TrainService;
using Microsoft.AspNetCore.Mvc;


namespace EAD_Web_Services.Controllers.TrainController
{
    /// <summary>
    /// Controller for managing train-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService trainService;

        public TrainController(ITrainService trainService)
        {
            this.trainService = trainService;
        }

        /// <summary>
        /// Get a list of all trains.
        /// </summary>
        /// <returns>A list of Train objects.</returns>
        [HttpGet]
        public ActionResult<List<Train>> Get()
        {
            return trainService.Get();
           
        }

        /// <summary>
        /// Get a train by its ID.
        /// </summary>
        /// <param name="id">The ID of the train to retrieve.</param>
        /// <returns>The Train object if found; otherwise, returns NotFound.</returns>
        [HttpGet("{id}")]
        public ActionResult<Train> Get(string id)
        {
            var train = trainService.Get(id);

            if (train == null)
            {
                return NotFound($"Train with id = {id} not found "); ;
            }

            return train;
        }


        /// <summary>
        /// Search for trains by departure and arrival locations.
        /// </summary>
        /// <param name="trainsRequestBody">The request body containing departure and arrival information.</param>
        /// <returns>A list of TrainsResponseBody objects matching the search criteria.</returns>
        /// 
        [HttpPost("search")]
        public ActionResult<List<TrainsResponseBody>> GetByDepartureAndArrival([FromBody] TrainsRequestBody trainsRequestBody)
        {
            var trains = trainService.GetByDepartureAndArrival(trainsRequestBody);

            if (trains == null)
            {
                return NotFound($"Train with departure = {trainsRequestBody.Departure} and arrival = {trainsRequestBody.Arrival} not found "); ;
            }

            return trains;
        }


        /// <summary>
        ///create a new train.
        /// </summary>
        /// <param name="train">The Train object to create.</param>
        /// <returns> created Train object.</returns>
        [HttpPost]
        public ActionResult<Train> Post([FromBody] Train train)
        {
            trainService.Create(train);

            return CreatedAtAction(nameof(Get), new { id = train.Id }, train);
        }

        /// <summary>
        /// Update and existing train by its Id 
        /// </summary>
        /// <param name="id">The ID of the train to update</param>
        /// <param name="train">The Train object with updated information.</param>
        /// <returns>NoContent if the train is updated successfully; otherwise, NotFound.</returns>
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Train train)
        {
            var trainToUpdate = trainService.Get(id);

            if (trainToUpdate == null)
            {
                return NotFound($"Train with id = {id} not found ");
            }

            trainService.Update(id, train);

            return NoContent();
        }

        /// <summary>
        /// Delete a train by its ID.
        /// </summary>
        /// <param name="id">The ID of the train to delete.</param>
        /// <returns>return Ok if the train is deleted successfully; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var trainToDelete = trainService.Get(id);

            if (trainToDelete == null)
            {
                return NotFound($"Train with id = {id} not found ");
            }

            trainService.Remove(trainToDelete.Id);

            return Ok($"Train with Id = {id} deleted");
        }

        /// <summary>
        /// Update the active status of a train by its ID.
        /// </summary>
        /// <param name="id">The ID of the train to update.</param>
        /// <returns>ActionResult indicating the status update result.</returns>
        [HttpPatch("updateActiveStatus/{id}")]
        public ActionResult UpdateTrainsActiveStatus(string id)
        {
            var trainToUpdate = trainService.Get(id);

            if (trainToUpdate == null)
            {
                return NotFound($"Train with id = {id} not found ");
            }

            var result =  trainService.UpdateTrainsActiveStatus(id);

            return Ok(result);
        }
    }
}
