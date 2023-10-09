//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260460
//   Name  :  Piumika Saranga H.A.

using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;

namespace EAD_Web_Services.CommonService
{
    /// <summary>
    /// / Common service interface for handling train-related operations.
    /// </summary>
    public interface ICommonService
    {
        List<Reservation> GetByTrainIdAndDate(string trainId, DateTime date);
        int GetAvailableSeats(string trainId, DateTime date, int seatCount);
        double CalculatePrice(Train train, string departure, string arrival, Station departureStation, Station arrivalStation);
        Train Get(string id);
    }
}
