//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260460 , IT20032838
//   Name  :  Piumika Saranga H.A. , Devsrini Savidya A.S.

using EAD_Web_Services.Models.ReservationModel;

namespace EAD_Web_Services.Services.ReservationService
{
    /// <summary>
    /// Interface for the Reservation Service
    /// </summary>
    public interface IReservationService
    {
        List<Reservation> Get();
        Reservation Get(string id);
        //get by train id and date
        List<Reservation> GetByTrainIdAndDate(string trainId, DateTime date);
        Reservation Create(Reservation reservation);
        string Update(string id, Reservation reservation , ReservationUpdateBody reservationUpdateBody);
        string Remove(string id,DateTime date);

    }
}
