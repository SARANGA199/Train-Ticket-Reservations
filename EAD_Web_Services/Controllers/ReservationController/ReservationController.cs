using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Services.ReservationService;
using Microsoft.AspNetCore.Mvc;


namespace EAD_Web_Services.Controllers.ReservationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {

        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        // GET: api/<ReservationController>
        [HttpGet]
        public ActionResult<List<Reservation>> Get()
        {
            return reservationService.Get();
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(string id)
        {
            var reservation = reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound($"Reservation with id = {id} not found "); ;
            }
            return reservation;
        }

        //get by train id and date
        [HttpPost("reservations")]
        public ActionResult<List<Reservation>> GetByTrainIdAndDate([FromBody]ReservationsRequestBody reservationsRequestBody)
        {
            var reservations = reservationService.GetByTrainIdAndDate(reservationsRequestBody.TrainId, reservationsRequestBody.Date);

            if (reservations == null)
            {
                return NotFound($"Reservation with train id = {reservationsRequestBody.TrainId} and date = {reservationsRequestBody.Date} not found "); ;
            }
            return reservations;
        }

        

        // POST api/<ReservationController>
        [HttpPost]
        public ActionResult<Reservation> Post([FromBody] Reservation reservation)
        {
            reservationService.Create(reservation);

            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, reservation);
        }

        // PUT api/<ReservationController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Reservation reservation)
        {
            var reservationToUpdate = reservationService.Get(id);

            if (reservationToUpdate == null)
            {
                return NotFound($"Reservation with id = {id} not found "); ;
            }

            reservationService.Update(id, reservation);

            return NoContent(); 
        }

        // DELETE api/<ReservationController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var reservation = reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound($"Reservation with id = {id} not found "); ;
            }

           var result =   reservationService.Remove(id,reservation.Date);

            return Ok(result);
        }
        
    }
}
