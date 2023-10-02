using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.ReservationService;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.CommonService
{
    public class CommonService : ICommonService
    {
        private readonly IMongoCollection<Reservation> _reservation;
        private readonly IMongoCollection<Train> _trains;

        public CommonService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationsCollectionName);
            _trains = database.GetCollection<Train>(settings.TrainsCollectionName);

        }

        public double CalculatePrice(Train train, string departure, string arrival, Station departureStation, Station arrivalStation)
        {

            var trainTypeCharge = train.TrainTypesDetails.Price;
            var numberOfStations = train.Stations?.Count(station => station.StationName == departure || station.StationName == arrival);

            var timeBetweenDepartureAndArrival = arrivalStation?.Time - departureStation?.Time;

            var price = trainTypeCharge * ((numberOfStations ?? 0) + (timeBetweenDepartureAndArrival?.TotalHours ?? 0));

            return price;
        }

        public Train Get(string id)
        {
            return _trains.Find(train => train.Id == id).FirstOrDefault();
        }

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
