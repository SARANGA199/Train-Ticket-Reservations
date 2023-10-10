//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260460
//   Name  :  Piumika Saranga H.A.

using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.ReservationService;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.CommonService
{
    /// <summary>
    /// Common service class for handling common train-related operations.
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly IMongoCollection<Reservation> _reservation;
        private readonly IMongoCollection<Train> _trains;

        /// <summary>
        /// Initializes a new instance of the CommonService class with database settings and a MongoDB client.
        /// </summary>
        /// <param name="settings">The database settings.</param>
        /// <param name="mongoClient">The MongoDB client.</param>
        public CommonService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationsCollectionName);
            _trains = database.GetCollection<Train>(settings.TrainsCollectionName);

        }

        /// <summary>
        /// Calculate the price for a train journey between two stations.
        /// </summary>
        /// <param name="train"></param>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="departureStation"></param>
        /// <param name="arrivalStation"></param>
        /// <returns>The calculated price for the journey.</returns>
        public double CalculatePrice(Train train, string departure, string arrival, Station departureStation, Station arrivalStation)
        {

            var trainTypeCharge = train.TrainTypesDetails.Price;
            var numberOfStations = train.Stations?.Count(station => station.StationName == departure || station.StationName == arrival);

            var timeBetweenDepartureAndArrival = arrivalStation?.Time - departureStation?.Time;

            var price = trainTypeCharge * ((numberOfStations ?? 0) + (timeBetweenDepartureAndArrival?.TotalHours ?? 0));

            return price;
        }

        /// <summary>
        /// Get a train by its ID.
        /// </summary>
        /// <param name="id">The ID of the train.</param>
        /// <returns>The train object if found; otherwise, returns null.</returns>
        public Train Get(string id)
        {
            return _trains.Find(train => train.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get the available seats for a specific train on a given date.
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="date"></param>
        /// <param name="seatCount"></param>
        /// <returns>The number of available seats for the specified train and date.</returns>
        public int GetAvailableSeats(string trainId, DateTime date, int seatCount)
        {
            //get reservations
            var currentReservations = GetByTrainIdAndDate(trainId, date);

            //get total number of passengers
            var totalReservedSeats = currentReservations.Sum(reservation => reservation.PassengersCount);

            //get available seats
            var availableSeats = seatCount - totalReservedSeats;

            return availableSeats;
        }

        /// <summary>
        /// Get reservations by train ID and date.
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="date"></param>
        /// <returns>A list of reservations for the specified train and date.</returns>
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
    }
}
