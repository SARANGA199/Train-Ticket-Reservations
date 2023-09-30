namespace EAD_Web_Services.Models.TrainModel
{
    public interface ITrainStoreDatabaseSettings
    {
        string TrainsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
