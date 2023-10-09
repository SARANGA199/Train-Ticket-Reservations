//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20032838
//   Name  :  Devsrini Savidya A.S.

using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.RequestAgentModel;
using MongoDB.Driver;

namespace EAD_Web_Services.Services.RequestAgentService
{
    /// <summary>
    /// Service class for managing RequestAgent objects.
    /// </summary>
    public class RequestAgentService : IRequestAgentService
    {
        private readonly IMongoCollection<RequestAgent> _requestAgent;

        /// <summary>
        /// Initializes a new instance of the RequestAgentService class.
        /// </summary>
        /// <param name="settings">The database setting</param>
        /// <param name="mongoClient">The MongoDB Client</param>
        public RequestAgentService(IDatabaseSettings settings ,IMongoClient mongoClient )
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _requestAgent = database.GetCollection<RequestAgent>(settings.RequestAgentCollectionName);
            
        }

        /// <summary>
        /// Create a new request Agent
        /// </summary>
        /// <param name="requestAgent">The requestAgent object to create</param>
        /// <returns>The created RequestAgent</returns>
        public RequestAgent Create(RequestAgent requestAgent)
        {
             _requestAgent.InsertOne(requestAgent);
            return requestAgent;
        }

        /// <summary>
        /// Get a list of all request agents
        /// </summary>
        /// <returns>A list of RequestAgent Objects</returns>
        public List<RequestAgent> Get()
        {
            return _requestAgent.Find(requestAgent => true).ToList();
        }

        /// <summary>
        /// Get a Request Agent by its ID
        /// </summary>
        /// <param name="id">The ID of the request agent to retrieve</param>
        /// <returns>The RequestAgent object if found; otherwise, returns null.</returns>
        public RequestAgent Get(string id)
        {
            return _requestAgent.Find(requestAgent => requestAgent.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Remove a request agent by its ID
        /// </summary>
        /// <param name="id">The ID of the request agent to remove</param>
        /// <returns>Removal results</returns>
        public string Remove(string id)
        {
            _requestAgent.DeleteOne(requestAgent => requestAgent.Id == id);

            return "Request Deleted Successfully";

        }
    }
}
