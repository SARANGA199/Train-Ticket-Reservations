using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.RequestAgentModel;
using MongoDB.Driver;

namespace EAD_Web_Services.Services.RequestAgentService
{
    public class RequestAgentService : IRequestAgentService
    {
        private readonly IMongoCollection<RequestAgent> _requestAgent;

        public RequestAgentService(IDatabaseSettings settings ,IMongoClient mongoClient )
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _requestAgent = database.GetCollection<RequestAgent>(settings.RequestAgentCollectionName);
            
        }

        public RequestAgent Create(RequestAgent requestAgent)
        {
             _requestAgent.InsertOne(requestAgent);
            return requestAgent;
        }

        public List<RequestAgent> Get()
        {
            return _requestAgent.Find(requestAgent => true).ToList();
        }

        public RequestAgent Get(string id)
        {
            return _requestAgent.Find(requestAgent => requestAgent.Id == id).FirstOrDefault();
        }

        
        public string Remove(string id)
        {
            _requestAgent.DeleteOne(requestAgent => requestAgent.Id == id);

            return "Request Deleted Successfully";

        }
    }
}
