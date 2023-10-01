using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservation;

        public ReservationService(IDatabaseSettings settings, IMongoClient mongoClient) 
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationsCollectionName);
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

            //check if reserved date and current date date count is less than 5

            //console log the reservedDate.Date.Subtract(DateTime.Now.Date).Days
            Console.WriteLine($"reservedDate.Date.Subtract(DateTime.Now.Date).Days :: {reservedDate.Date.Subtract(DateTime.Now.Date).Days}");



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

        public void Update(string id, Reservation reservation)
        {
            _reservation.ReplaceOne(reservation => reservation.Id == id, reservation);
        }
    }
}
