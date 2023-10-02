using EAD_Web_Services.CommonService;
using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Services.TrainService;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservation;
        private readonly ITrainService trainService;

     

        public ReservationService(IDatabaseSettings settings, IMongoClient mongoClient , ITrainService trainService)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationsCollectionName);

            this.trainService = trainService;
            
        }

        public Reservation Create(Reservation reservation)
        {
            _reservation.InsertOne(reservation);
            return reservation;
        }

        public List<Reservation> Get()
        {
            return _reservation.Find(reservation => true).ToList();
        }

        public Reservation Get(string id)
        {
           return _reservation.Find(reservation => reservation.Id == id).FirstOrDefault();
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
                    {
                        //calculate the price
                        var price = trainService.CalculatePrice(train, reservationUpdateBody.Depature, reservationUpdateBody.Arrival, departureStation, arrivalStation);

                        //update the reservation
                        reservation.PassengersCount = reservationUpdateBody.PassengersCount;
                        reservation.Date = reservationUpdateBody.Date;
                        reservation.Depature = reservationUpdateBody.Depature;
                        reservation.Arrival = reservationUpdateBody.Arrival;

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
