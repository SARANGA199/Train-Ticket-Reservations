using EAD_Web_Services.Models.TrainModel;

namespace EAD_Web_Services.Services.TrainService
{
    public interface ITrainService
    {
        List<Train> Get();
        Train Get(string id);
        Train Create(Train train);
        void Update(string id, Train train);
        void Remove(string id);
        void UpdateStatus(string id);
    }
}
