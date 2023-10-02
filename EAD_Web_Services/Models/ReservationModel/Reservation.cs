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

        [BsonElement("depature")]
        public string Depature { get; set; } = string.Empty;

        [BsonElement("arrival")]
        public string Arrival { get; set; } = string.Empty;

        [BsonElement("depature_time")]
        public string DepatureTime { get; set; } = string.Empty;

        [BsonElement("arrival_time")]
        public string ArrivalTime { get; set; } = string.Empty;

        [BsonElement("average_time_duration")]
        public string AverageTimeDuration { get; set; } = string.Empty;

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

    public class ReservationUpdateBody
    {
        public int PassengersCount { get; set; }
        public DateTime Date { get; set; } = DateTime.MinValue;
        public string Depature { get; set; } = string.Empty;
        public string Arrival { get; set; } = string.Empty;

    }
}
