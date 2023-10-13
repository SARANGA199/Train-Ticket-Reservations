//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260910 , IT20032838
//   Name  :  Vishwa J.W.P , Devsrini Savidya A.S.


using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.ReservationService;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.Services.TrainService
{
    /// <summary>
    /// Service class for managing Train objects.
    /// </summary>
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Train> _trains;
        private readonly IMongoCollection<Reservation> _reservation;

        /// <summary>
        /// Initializes a new instance of the TrainService class.
        /// </summary>
        /// <param name="settings">The database settings.</param>
        /// <param name="mongoClient">The MongoDB client.</param>
        public TrainService(IDatabaseSettings settings , IMongoClient mongoClient )
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _trains = database.GetCollection<Train>(settings.TrainsCollectionName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationsCollectionName);

        }

        /// <summary>
        /// Create a new train.
        /// </summary>
        /// <param name="train">The Train object to create.</param>
        /// <returns>The created Train object.</returns>
        public Train Create(Train train)
        {

            _trains.InsertOne(train);
            return train;
        }

        /// <summary>
        /// Get a list of all trains.
        /// </summary>
        /// <returns>A list of Train objects.</returns>
        public List<Train> Get()
        {
            return _trains.Find(train => true).ToList();
        }

        /// <summary>
        /// Get a train by its ID.
        /// </summary>
        /// <param name="id">The ID of the train to retrieve.</param>
        /// <returns>The Train object if found; otherwise, returns null.</returns>
        public Train Get(string id)
        {
            return _trains.Find(train => train.Id == id).FirstOrDefault();
        }


        /// <summary>
        /// Get a list of trains filtered by departure and arrival stations.
        /// </summary>
        /// <param name="trainsRequestBody">The request body containing departure , arrival stations , current data and number of seats.</param>
        /// <returns>A list of filtered Train objects.</returns>
        public List<TrainsResponseBody> GetByDepartureAndArrival(TrainsRequestBody trainsRequestBody)
        {
            List<TrainsResponseBody> filteredTrains = new();

            
            //check if train is active
            var trains = FilterTrainsByDepartureAndArrival(trainsRequestBody.Departure, trainsRequestBody.Arrival);

           
            //loop through trains and get reservations
            foreach (var train in trains)
            {   

                //get available seats
               var availableSeats = GetAvailableSeats(train.Id, trainsRequestBody.Date, train.SeatCount);

                //check if available seats are greater than or equal to requested seats
                if (availableSeats >= trainsRequestBody.SeatCount)
                {

                    // Find the departure station
                    var departureStation = train.Stations?.FirstOrDefault(station => station.StationName == trainsRequestBody.Departure);

                    // Find the arrival station
                    var arrivalStation = train.Stations?.FirstOrDefault(station => station.StationName == trainsRequestBody.Arrival);

                    //calculate price
                    var price = CalculatePrice(train, trainsRequestBody.Departure, trainsRequestBody.Arrival, departureStation, arrivalStation);


                     //convert departure time to time only and set AM/PM from local time
                     var dep_time = departureStation?.Time.ToString("hh:mm tt");

                    //convert arrival time to time only and set AM/PM from local time
                    var arrival_time = arrivalStation?.Time.ToString("hh:mm tt");
             
                    //add train to filtered trains
                    filteredTrains.Add(new TrainsResponseBody
                    {
                        TrainId = train.Id,
                        TrainName = train.TrainName,
                        Departure = trainsRequestBody.Departure,
                        Arrival = trainsRequestBody.Arrival,
                        DepartureTime = dep_time,
                        ArrivalTime = arrival_time,
                        TripTimeDuration = (arrivalStation?.Time - departureStation?.Time)?.TotalHours ?? 0,
                        AvailableSeatCount = availableSeats,
                        RequestedSeatCount = trainsRequestBody.SeatCount,
                        Date = trainsRequestBody.Date,
                        Amount = price
                        
                    });
                    
                }
                
            }

            return filteredTrains;
        }

        /// <summary>
        /// Calculate price for a train journey.
        /// </summary>
        /// <param name="train"></param>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="departureStation"></param>
        /// <param name="arrivalStation"></param>
        /// <returns></returns>
        public double CalculatePrice(Train train, string departure, string arrival, Station departureStation, Station arrivalStation)
        {

            var trainTypeCharge = train.TrainTypesDetails.Price;
            var numberOfStations = train.Stations?.Count(station => station.StationName == departure || station.StationName == arrival);

            var timeBetweenDepartureAndArrival = arrivalStation?.Time - departureStation?.Time;

            var price = trainTypeCharge * ((numberOfStations ?? 0) + (timeBetweenDepartureAndArrival?.TotalHours ?? 0));

            return price;
        }

        /// <summary>
        /// Get the number of available seats for a train on a specific date.
        /// </summary>
        /// <param name="trainId">The ID of the train</param>
        /// <param name="date">The date of the journey.</param>
        /// <param name="seatCount">Count of the seats</param>
        /// <returns></returns>
        public int GetAvailableSeats(string trainId, DateTime date, int seatCount)
        {

            //get reservations
            var currentReservations = GetTrainByIdDate(trainId, date);

            //get total number of passengers
            var totalReservedSeats = currentReservations.Sum(reservation => reservation.PassengersCount);

            //get available seats
            var availableSeats = seatCount - totalReservedSeats;

            return availableSeats;
        }

        /// <summary>
        /// Get reservations for a train on a specific date.
        /// </summary>
        /// <param name="trainId">The ID of the train.</param>
        /// <param name="date">The date of the journey.</param>
        /// <returns>A list of Reservation objects.</returns>
        public List<Reservation> GetTrainByIdDate(string trainId, DateTime date)
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
        /// Filter trains by departure and arrival stations.
        /// </summary>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <returns></returns>
        private List<Train> FilterTrainsByDepartureAndArrival(string departure, string arrival)
        {
            // Create a combined filter expression.
            Expression<Func<Train, bool>> combinedFilter = train =>
                train.Stations != null &&
                train.Stations.Any(station => station.StationName == departure) &&
                train.Stations.Any(station => station.StationName == arrival) &&
                train.Stations.Any(station => station.StationName == departure &&
                                              station.Time < train.Stations.First(s => s.StationName == arrival).Time) &&
                train.IsActive; // Check if the train is active

            // Apply the combined filter using the Where method.
            var filteredTrains = _trains.AsQueryable().Where(combinedFilter).ToList();

            return filteredTrains;
        }

        /// <summary>
        /// Remove a train by its ID.
        /// </summary>
        /// <param name="id">The ID of the train to remove.</param>
        public void Remove(string id)
        {
            _trains.DeleteOne(train => train.Id == id);
        }

        /// <summary>
        /// Update a train.
        /// </summary>
        /// <param name="id">The ID of the train to update.</param>
        /// <param name="train">The Train object with updated information.</param>
        public void Update(string id, Train train)
        {
            _trains.ReplaceOne(train => train.Id == id, train);
        }

        /// <summary>
        /// Update the active status of a train.
        /// </summary>
        /// <param name="id">The ID of the train to update.</param>
        /// <returns>A message indicating the update result.</returns>
        public string UpdateTrainsActiveStatus(string id)
        {
            var train = _trains.Find(train => train.Id == id).FirstOrDefault();

            //get reservations from today onwards with this train id
            var reservations = CheckFutureReservation(id, DateTime.Now);

            //if there are reservations then prevent updating the status
            if (reservations.Count <= 0)
            {
                train.IsActive = !train.IsActive;
                _trains.UpdateOne(train => train.Id == id, Builders<Train>.Update.Set("IsActive", train.IsActive));

                //return message with status
                return $"Train status updated to {train.IsActive}";
            }else
            {
                //return message with status
                return $"This Train has reservations from today onwards, status cannot be updated";
            }
        }

        /// <summary>
        /// Get future reservations for a train with a specific date.
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Reservation> CheckFutureReservation(string trainId, DateTime date)
        {
            // Create a combined filter expression.
            Expression<Func<Reservation, bool>> combinedFilter = reservation =>
                reservation.TrainId == trainId &&
                //check only the date part of the date time
                reservation.Date.Date >= date.Date;

            // Apply the combined filter using the Where method.
            var filteredReservations = _reservation.AsQueryable().Where(combinedFilter).ToList();
            return filteredReservations;
        }   


    }
}
