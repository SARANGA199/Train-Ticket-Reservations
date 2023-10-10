//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20236014
//   Name  :  Ravindu Yasith T.K.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Web_Services.Models.UserModel
{
    /// <summary>
    /// Represent a User Document in MongoDB
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        public string Nic { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("user_role")]
        public UserRole UserRole { get; set; } =  UserRole.Traveler;

        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;

        // Enable timestamp for user document
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }

    /// <summary>
    /// Enumeration of user roles.
    /// </summary>
    public enum UserRole
    {
        BackOfficeUser = 1,
        TravelAgent    = 2,
        Traveler       = 3
    }

    /// <summary>
    /// Represents the request body for user login.
    /// </summary>
    public class LoginRequest
    {
        public string Nic { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
