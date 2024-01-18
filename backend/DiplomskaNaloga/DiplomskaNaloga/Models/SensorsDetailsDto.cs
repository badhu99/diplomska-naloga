using System;
using static DiplomskaNaloga.Services.SensorDataService;

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
        //public string SensorHash { get; set; }
    }

    public class SensorDetailsContent
	{
        public string Name { get; set; }
        public List<dynamic> Series { get; set; }
    }

    public class SensorDetailsResponse
    {
        public List<SensorDetailsContent> Content { get; set; }
        public string Name { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; }
        public Guid UserId { get; set; }
        public string SensorHash { get; set; }

    }
}

