//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20032838
//   Name  :  Devsrini Savidya A.S.

namespace EAD_Web_Services.DatabaseConfiguration
{
    /// <summary>
    ///  Represents an interface for database settings required to configure a MongoDB database connection.
    /// </summary>
    public interface IDatabaseSettings
    {
        
        string ConnectionString { get; set; } 
        string DatabaseName { get; set; }
        string TrainsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ReservationsCollectionName { get; set; }
        


    }
}
