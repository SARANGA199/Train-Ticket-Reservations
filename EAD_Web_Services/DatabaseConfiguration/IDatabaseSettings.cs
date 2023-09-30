namespace EAD_Web_Services.DatabaseConfiguration
{
    public interface IDatabaseSettings
    {
        string TrainsCollectionName { get; set; }
        string ConnectionString { get; set; } 
        string DatabaseName { get; set; }
    }
}
