//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260460 , IT20032838
//   Name  :  Piumika Saranga H.A. , Devsrini Savidya A.S.

using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Services.ReservationService;
using Microsoft.AspNetCore.Mvc;


namespace EAD_Web_Services.Controllers.ReservationController
{
    /// <summary>
    /// Controller for managing reservation-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {

        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        /// <summary>
        /// Get a list of all reservations.
        /// </summary>
        /// <returns>a list of reservations objects</returns>
        [HttpGet]
        public ActionResult<List<Reservation>> Get()
        {
            return reservationService.Get();
        }

        /// <summary>
        /// Get a reservation by its id
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>The Reservation object if found; otherwise, returns NotFound.</returns>
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

        /// <summary>
        /// Create a new reservation.
        /// </summary>
        /// <param name="reservation">The Reservation object to create.</param>
        /// <returns>The created Reservation object.</returns>
        [HttpPost]
        public ActionResult<Reservation> Post([FromBody] Reservation reservation)
        {
            reservationService.Create(reservation);

            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, reservation);
        }

        /// <summary>
        /// Update an existing reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservationUpdateBody">The ReservationUpdateBody object with updated information.</param>
        /// <returns>ActionResult indicating the update result.</returns>
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] ReservationUpdateBody reservationUpdateBody)
        {
            var reservationToUpdate = reservationService.Get(id);

            if (reservationToUpdate == null)
            {
                return NotFound($"Reservation with id = {id} not found "); ;
            }

            var result = reservationService.Update(id, reservationToUpdate , reservationUpdateBody);

            return Ok(result); 
        }

        /// <summary>
        /// Delete a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>ActionResult indicating the deletion result</returns>
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
