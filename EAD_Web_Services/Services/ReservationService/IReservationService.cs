using EAD_Web_Services.Models.ReservationModel;

namespace EAD_Web_Services.Services.ReservationService
{
    public interface IReservationService
    {
        List<Reservation> Get();
        Reservation Get(string id);
        //get by train id and date
        List<Reservation> GetByTrainIdAndDate(string trainId, DateTime date);
        Reservation Create(Reservation reservation);
        void Update(string id, Reservation reservation);
        void Remove(string id);

    }
}
