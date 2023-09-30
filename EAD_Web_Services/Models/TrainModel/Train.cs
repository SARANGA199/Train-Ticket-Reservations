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


    }

    public class Station
    {
        [BsonElement("station_name")]
        public string StationName { get; set; } = string.Empty;

        [BsonElement("time")]
        public string Time { get; set; } = string.Empty;
    }
}
