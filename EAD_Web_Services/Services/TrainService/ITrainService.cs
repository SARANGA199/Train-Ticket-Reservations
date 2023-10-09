//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260910
//   Name  :  Vishwa J.W.P

using EAD_Web_Services.Models.TrainModel;

namespace EAD_Web_Services.Services.TrainService
{
    /// <summary>
    /// Interface for the Train service.
    /// </summary>
    public interface ITrainService
    {
        List<Train> Get();
        Train Get(string id);
        Train Create(Train train);
        void Update(string id, Train train);
        void Remove(string id);
        string UpdateTrainsActiveStatus(string id);
        List<TrainsResponseBody> GetByDepartureAndArrival(TrainsRequestBody trainsRequestBody);
       int GetAvailableSeats(string trainId, DateTime date, int seatCount);
       double CalculatePrice(Train train, string departure, string arrival, Station departureStation, Station arrivalStation);
       

    }
}
