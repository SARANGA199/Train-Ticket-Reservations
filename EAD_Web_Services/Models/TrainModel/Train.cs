//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20260910
//   Name  :  Vishwa J.W.P
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace EAD_Web_Services.Models.TrainModel
{
    /// <summary>
    /// Represent a Train Document in MongoDB
    /// </summary>
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

        [BsonElement("train_types_details")]
        public TrainTypesDetails TrainTypesDetails { get; set; } = new TrainTypesDetails();


        [BsonElement("stations")]
        public List<Station>? Stations { get; set; }

        // Enable timestamp for train document
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    }
    /// <summary>
    /// Represent a Train Station Document in MongoDB
    /// </summary>
    public class Station
    {
        [BsonElement("station_name")]
        public string StationName { get; set; } = string.Empty;

        [BsonElement("time")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Time { get; set; } = DateTime.MinValue;
    }

    /// <summary>
    /// Represents train types with prices
    /// </summary>
    public class TrainTypesDetails
    {
        [BsonElement("train_type")]
        public TrainType TrainType { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }
    }

    /// <summary>
    /// train types enum
    /// </summary>
    public enum TrainType
    {
        Express = 1,
        Commuter = 2,
        InterCity = 3
    }

    /// <summary>
    /// Represents the request body for searching trains.
    /// </summary>
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

    /// <summary>
    /// Represents the response body for available trains
    /// </summary>
    public class TrainsResponseBody
    {
        [BsonElement("train_id")]
        public string TrainId { get; set; } = string.Empty;

        [BsonElement("train_name")]
        public string TrainName { get; set; } = string.Empty;

        [BsonElement("departure")]
        public string Departure { get; set; } = string.Empty;

        [BsonElement("arrival")]
        public string Arrival { get; set; } = string.Empty;

        [BsonElement("departure_time")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DepartureTime { get; set; } = DateTime.MinValue;

        [BsonElement("arrival_time")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ArrivalTime { get; set; } = DateTime.MinValue;

        //trip time in hours
        [BsonElement("trip_time_duration")]
        public double TripTimeDuration { get; set; }

        [BsonElement("requested_seat_count")]
        public int RequestedSeatCount { get; set; }

        [BsonElement("available_seat_count")]
        public int AvailableSeatCount { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; } = 2500;
    }

}
