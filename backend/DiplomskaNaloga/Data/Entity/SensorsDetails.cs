using MongoDB.Bson.Serialization.Attributes;

namespace Data.Entity
{
    [BsonIgnoreExtraElements]
    public class SensorsDetails
	{
        public DateTime DateTime { get; set; }
        public string SensorGroupId { get; set; }
        public dynamic Body { get; set; }
    }
}

