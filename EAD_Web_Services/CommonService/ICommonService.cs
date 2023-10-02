using EAD_Web_Services.Models.ReservationModel;
using EAD_Web_Services.Models.TrainModel;

namespace EAD_Web_Services.CommonService
{
    public interface ICommonService
    {
        List<Reservation> GetByTrainIdAndDate(string trainId, DateTime date);
        int GetAvailableSeats(string trainId, DateTime date, int seatCount);
        double CalculatePrice(Train train, string departure, string arrival, Station departureStation, Station arrivalStation);
        Train Get(string id);
    }
}
