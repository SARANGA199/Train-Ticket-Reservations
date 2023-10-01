namespace EAD_Web_Services.DatabaseConfiguration
{
    public class DatabaseSettings : IDatabaseSettings
    {
        
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string TrainsCollectionName { get; set; } = string.Empty;
        public string UsersCollectionName { get; set; } = string.Empty;
        public string ReservationsCollectionName { get; set; } = string.Empty;
    }
}
