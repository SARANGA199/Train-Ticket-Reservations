using EAD_Web_Services.Models.TrainModel;
using MongoDB.Driver;

namespace EAD_Web_Services.Services.TrainService
{
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Train> _trains;

        public TrainService(ITrainStoreDatabaseSettings settings , IMongoClient mongoClient)
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
