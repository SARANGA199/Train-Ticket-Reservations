using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.TrainModel;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EAD_Web_Services.Services.TrainService
{
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Train> _trains;

        public TrainService(IDatabaseSettings settings , IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _trains = database.GetCollection<Train>(settings.TrainsCollectionName);
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

        public List<Train> GetByDepartureAndArrival(TrainsRequestBody trainsRequestBody)
        {
            Console.WriteLine($"request :: {trainsRequestBody.Departure}");

            // Call function to filter trains by departure and arrival.
            var trains = FilterTrainsByDepartureAndArrival(trainsRequestBody.Departure, trainsRequestBody.Arrival);

            // Check if trains exist.
            return trains;
        }

        private List<Train> FilterTrainsByDepartureAndArrival(string departure, string arrival)
        {
            // Create a combined filter expression.
            Expression<Func<Train, bool>> combinedFilter = train =>
                train.Stations != null &&
                train.Stations.Any(station => station.StationName == departure) &&
                train.Stations.Any(station => station.StationName == arrival) &&
                train.Stations.Any(station => station.StationName == departure &&
                                              station.Time < train.Stations.First(s => s.StationName == arrival).Time);

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

        public void UpdateStatus(string id)
        {
            var train = _trains.Find(train => train.Id == id).FirstOrDefault();
            train.IsActive = !train.IsActive;
            _trains.UpdateOne(train => train.Id == id, Builders<Train>.Update.Set("IsActive", train.IsActive));
        }


    }
}
