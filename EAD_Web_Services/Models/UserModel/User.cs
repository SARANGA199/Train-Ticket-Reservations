using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Web_Services.Models.UserModel
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonElement("nic")]
        public string Nic { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("isTraveler")]
        public bool IsTraveler { get; set; } = true;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    
    }

    public class LoginRequest
    {
        public string Nic { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
