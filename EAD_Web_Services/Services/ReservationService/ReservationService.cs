using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;
using MongoDB.Driver;

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

        public void Remove(string id)
        {
            _reservation.DeleteOne(reservation => reservation.Id == id);
        }

        public void Update(string id, Reservation reservation)
        {
            _reservation.ReplaceOne(reservation => reservation.Id == id, reservation);
        }
    }
}
