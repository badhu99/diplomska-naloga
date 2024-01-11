using System;
namespace DiplomskaNaloga.Models
{
    public class SensorGroupDto: SensorGroupData
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string SensorHash { get; set; }
    }

    public class SensorGroupData
    {
        public string Name { get; set; }
        public string ColumnX { get; set; }
        public string ColumnY { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}

