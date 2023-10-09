//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20032838
//   Name  :  Devsrini Savidya A.S.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace EAD_Web_Services.Models.RequestAgentModel
{
    /// <summary>
    /// Represent a RequestAgent document in MongoDB
    /// </summary>
    public class RequestAgent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("nic")]
        public string Nic { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("number")]
        public string Number { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;


        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    }
}
