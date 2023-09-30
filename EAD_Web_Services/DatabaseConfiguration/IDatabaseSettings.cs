namespace EAD_Web_Services.DatabaseConfiguration
{
    public interface IDatabaseSettings
    {
        
        string ConnectionString { get; set; } 
        string DatabaseName { get; set; }
        string TrainsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
    }
}
