//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260460 , IT20032838
//   Name  :  Piumika Saranga H.A., Devsrini Savidya A.S.

using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.TrainService;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.Services.ReservationService
{
    /// <summary>
    /// Service class for managing Reservation objects.
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservation;
        private readonly ITrainService trainService;


        /// <summary>
        /// Initializes a new instance of the ReservationService class.
        /// </summary>
        /// <param name="settings">The database setting</param>
        /// <param name="mongoClient">The MongoDB Client</param>
        /// <param name="trainService">The train service.</param>
        public ReservationService(IDatabaseSettings settings, IMongoClient mongoClient , ITrainService trainService)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationsCollectionName);

            this.trainService = trainService;
            
        }

        /// <summary>
        /// Create a new reservation.
        /// </summary>
        /// <param name="reservation">The Reservation object to create.</param>
        /// <returns>The created Reservation object.</returns>
        public Reservation Create(Reservation reservation)
        {
            _reservation.InsertOne(reservation);
            return reservation;
        }

        /// <summary>
        /// Get a list of all reservations.
        /// </summary>
        /// <returns>A list of Reservation objects.</returns>
        public List<Reservation> Get()
        {
            return _reservation.Find(reservation => true).ToList();
        }

        /// <summary>
        /// Get a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>The Reservation object if found; otherwise, returns null.</returns>
        public Reservation Get(string id)
        {
           return _reservation.Find(reservation => reservation.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get a reservations by user nic.
        /// </summary>
        /// <param name="nic">The NIC of the user use to fetch reservations.</param>
        /// <returns>The Reservations list if found; otherwise, returns null.</returns>
        public List<Reservation> GetByNic(string nic)
        {
            return _reservation.Find(reservation => reservation.Nic == nic).ToList();
        }

        /// <summary>
        /// Get a reservation by train ID and date.
        /// </summary>
        /// <param name="trainId">The train ID</param>
        /// <param name="date">The date of the reservations</param>
        /// <returns><A list of reservation objects matching the criteria/returns>
        public List<Reservation> GetByTrainIdAndDate(string trainId, DateTime date)
        {

            //console log the train id and date
            Console.WriteLine($"train id :: {trainId}");
            Console.WriteLine($"date :: {date}");

            // Create a combined filter expression.
            Expression<Func<Reservation, bool>> combinedFilter = reservation =>
                reservation.TrainId == trainId &&
                //check only the date part of the date time
                reservation.Date.Date == date.Date;

            // Apply the combined filter using the Where method.
            var filteredReservations = _reservation.AsQueryable().Where(combinedFilter).ToList();
            Console.WriteLine($"filtered reservations :: {filteredReservations.Count}");
            return filteredReservations;
        }

        /// <summary>
        /// Remove a reservation by its ID and reserved date.
        /// </summary>
        /// <param name="id">The ID of the reservation to remove.</param>
        /// <param name="reservedDate">The date the reservation was made.</param>
        /// <returns>A string indicating the removal result.</returns>
        public string Remove(string id , DateTime reservedDate)
        {

            if (reservedDate.Date.Subtract(DateTime.Now.Date).Days > 5)
            {
                _reservation.DeleteOne(reservation => reservation.Id == id);
                return "Reservation cancelled successfully !";
            }
            else
            {
                //return error messge to controlle
                return "Cannot cancel reservation within 5 days of departure !";
            }

        }

        /// <summary>
        /// Update a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservation">The Reservation object to update.</param>
        /// <param name="reservationUpdateBody">The updated reservation data.</param>
        /// <returns>A string indicating the update result.</returns>
        public string Update(string id, Reservation reservation , ReservationUpdateBody reservationUpdateBody)
        {
            if (reservation.Date.Date.Subtract(DateTime.Now.Date).Days > 5)
            {
                Train train = trainService.Get(reservation.TrainId);
                var availableSeats = trainService.GetAvailableSeats(reservation.TrainId, reservationUpdateBody.Date, train.SeatCount );
                
                int newSeatCount = 0;
              
                //check whether new seat count is greater than the current seat count
                if (reservation.PassengersCount < reservationUpdateBody.PassengersCount)
                {
                    newSeatCount = reservationUpdateBody.PassengersCount - reservation.PassengersCount;
                   
                }

                if (availableSeats >= newSeatCount)
                {
                    //Find the departure station
                    var departureStation = train.Stations?.FirstOrDefault(station => station.StationName == reservationUpdateBody.Depature);

                    //Find the arrival station
                    var arrivalStation = train.Stations?.FirstOrDefault(station => station.StationName == reservationUpdateBody.Arrival);

                    //check these stations are available in the train
                    if (departureStation != null && arrivalStation != null)

                        //connvert the departure time to local time
                        //var dep_time = departureStation?.Time.ToString("hh:mm tt");
                    {
                        //calculate the price
                        var price = trainService.CalculatePrice(train, reservationUpdateBody.Depature, reservationUpdateBody.Arrival, departureStation, arrivalStation);

                        //update the reservation
                        reservation.PassengersCount = reservationUpdateBody.PassengersCount;
                        reservation.Date = reservationUpdateBody.Date;
                        reservation.Depature = reservationUpdateBody.Depature;
                        reservation.Arrival = reservationUpdateBody.Arrival;
                        reservation.DepatureTime = departureStation?.Time.ToString("hh:mm tt");
                        reservation.ArrivalTime = arrivalStation?.Time.ToString("hh:mm tt");
                        reservation.AverageTimeDuration = arrivalStation?.Time.Subtract(departureStation.Time).ToString();
                        reservation.TotalAmount = price * reservationUpdateBody.PassengersCount;
                        reservation.UpdatedAt = DateTime.Now;

                        _reservation.ReplaceOne(reservation => reservation.Id == id, reservation);

                        return "Reservation updated successfully !";
                    }
                    else
                    {
                        return "Invalid departure or arrival station !";
                    }


                }
                else
                {
                    return "No available seats on this day !";
                } 

            }
            else
            {
                return "Cannot update reservation within 5 days of departure !";
            }
        }
    }
}
