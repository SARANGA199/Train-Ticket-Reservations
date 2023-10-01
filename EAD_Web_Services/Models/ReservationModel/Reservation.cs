using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace EAD_Web_Services.Models.ReservationModel
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("reference_id")]
        public string ReferenceId { get; set; } = string.Empty;

        [BsonElement("nic")]
        public string Nic { get; set; } = string.Empty;

        [BsonElement("train_id")]
        public string TrainId { get; set; } = string.Empty;

        [BsonElement("passengers_count")]
        public int PassengersCount { get; set; }

        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; } = DateTime.MinValue;

        [BsonElement("total_amount")]
        public double TotalAmount { get; set; }

        // Enable timestamp for reservation document
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
    }

    public class ReservationsRequestBody
    {
        public string TrainId { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;
    }
}
