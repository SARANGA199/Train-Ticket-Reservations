using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace EAD_Web_Services.Models.TrainModel
{
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("train_name")]
        public string TrainName { get; set; } = string.Empty;

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("seat_count")]
        public int SeatCount { get; set; }

        [BsonElement("fee_per_station")]
        public double FeePerStation { get; set; }


        [BsonElement("stations")]
        public List<Station>? Stations { get; set; }

        // Enable timestamp for train document
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    }

    public class Station
    {
        [BsonElement("station_name")]
        public string StationName { get; set; } = string.Empty;

        [BsonElement("time")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Time { get; set; } = DateTime.MinValue;
    }

    //train request body
    public class TrainsRequestBody
    {
        [BsonElement("departure")]
        public string Departure { get; set; } = string.Empty;

        [BsonElement("arrival")]
        public string Arrival { get; set; } = string.Empty;

        [BsonElement("seat_count")]
        public int SeatCount { get; set; }

        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; } = DateTime.MinValue;
    }

}
