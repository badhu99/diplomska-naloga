using System;
namespace DiplomskaNaloga.Models
{
	public class SensorsDetailsDto:SensorDetailsData
	{
        public DateTime DateTime { get; set; }
		public Guid SensorGroupId { get; set; }
	}

	public class SensorDetailsData
    {
        public dynamic Body { get; set; }
    }
}

