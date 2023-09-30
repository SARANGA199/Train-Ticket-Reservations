namespace EAD_Web_Services.Models.TrainModel
{
    public class TrainStoreDatabaseSettings : ITrainStoreDatabaseSettings
    {
        public string TrainsCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
