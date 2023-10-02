﻿using EAD_Web_Services.CommonService;
using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.ReservationService;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.Services.TrainService
{
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Train> _trains;
        private readonly IMongoCollection<Reservation> _reservation;
       

        public TrainService(IDatabaseSettings settings , IMongoClient mongoClient )
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _trains = database.GetCollection<Train>(settings.TrainsCollectionName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationsCollectionName);

        }

        public Train Create(Train train)
        {

            _trains.InsertOne(train);
            return train;
        }

        public List<Train> Get()
        {
            return _trains.Find(train => true).ToList();
        }


        public Train Get(string id)
        {
            return _trains.Find(train => train.Id == id).FirstOrDefault();
        }



        public List<TrainsResponseBody> GetByDepartureAndArrival(TrainsRequestBody trainsRequestBody)
        {
            List<TrainsResponseBody> filteredTrains = new();

            // Call function to filter trains by departure and arrival. 
            //TODO: check if train is active
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
  
                    //add train to filtered trains
                    filteredTrains.Add(new TrainsResponseBody
                    {
                        TrainId = train.Id,
                        TrainName = train.TrainName,
                        Departure = trainsRequestBody.Departure,
                        Arrival = trainsRequestBody.Arrival,
                        DepartureTime = departureStation?.Time ?? DateTime.MinValue,
                        ArrivalTime = arrivalStation?.Time ?? DateTime.MinValue,
                        TripTimeDuration = (arrivalStation?.Time - departureStation?.Time)?.TotalHours ?? 0,
                        AvailableSeatCount = availableSeats,
                        RequestedSeatCount = trainsRequestBody.SeatCount,
                        Amount = price
                        
                    });
                    
                }
                
            }

            return filteredTrains;
        }

        //calculate price
        public double CalculatePrice(Train train, string departure, string arrival, Station departureStation, Station arrivalStation)
        {

            var trainTypeCharge = train.TrainTypesDetails.Price;
            var numberOfStations = train.Stations?.Count(station => station.StationName == departure || station.StationName == arrival);

            var timeBetweenDepartureAndArrival = arrivalStation?.Time - departureStation?.Time;

            var price = trainTypeCharge * ((numberOfStations ?? 0) + (timeBetweenDepartureAndArrival?.TotalHours ?? 0));

            return price;
        }

        //get available seats
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

        public void Remove(string id)
        {
            _trains.DeleteOne(train => train.Id == id);
        }

        public void Update(string id, Train train)
        {
            _trains.ReplaceOne(train => train.Id == id, train);
        }

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
