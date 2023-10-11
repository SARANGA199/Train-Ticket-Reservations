//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20032838
//   Name  :  Devsrini Savidya A.S.

namespace EAD_Web_Services.DatabaseConfiguration
{
    /// <summary>
    /// Represents the database settings required for configuring a MongoDB database connection.
    /// </summary>
    public class DatabaseSettings : IDatabaseSettings
    {
        
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string TrainsCollectionName { get; set; } = string.Empty;
        public string UsersCollectionName { get; set; } = string.Empty;
        public string ReservationsCollectionName { get; set; } = string.Empty;
     
    }
}
