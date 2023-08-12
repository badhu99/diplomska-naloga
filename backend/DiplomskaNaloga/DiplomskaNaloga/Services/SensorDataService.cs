using System.Dynamic;
using System.Text.Json;
using Data;
using Data.Entity;
using DiplomskaNaloga.Models;
using DiplomskaNaloga.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
namespace DiplomskaNaloga.Services
{
    public interface ISensorDataService {
		Task AddData(Guid sensorGroupId, Guid UserId, SensorDetailsData data);
		Task<dynamic> GetData(Guid sensorGroupId,Guid userId, int pageNumber, int pageSize);

    }
    public class SensorDataService : ISensorDataService
	{

		private readonly IMongoCollection<SensorsDetails> _sensorCollection;
		private readonly databaseContext _context;

        public SensorDataService(IOptions<MongoDbSettings> options, databaseContext context)
		{
			var settings = options.Value;

			var mongoClient = new MongoClient(settings.ConnectionString);
			var mongoDb = mongoClient.GetDatabase(settings.DatabaseName);

			_sensorCollection = mongoDb.GetCollection<SensorsDetails>(settings.CollectionName);
            _context = context;
        }

		public async Task<dynamic> GetData(Guid sensorGroupId, Guid userId, int pageNumber, int pageSize) {
			var sensorGroup = await _context.SensorGroups.FirstOrDefaultAsync(sg => sg.Id == sensorGroupId && sg.UserId == userId);
			if (sensorGroup == null) throw new UnauthorizedAccessException();

			var c = _sensorCollection
				.AsQueryable()
				.Where(sg => sg.SensorGroupId == sensorGroupId.ToString())
				.Select(sg => 
					sg.Body
                ).ToList();

			List<dynamic> list = new();
            foreach (var b in c) {
				var doc = BsonSerializer.Deserialize<dynamic>(b);
				list.Add(doc);
			}

			return list;
        }


		public async Task AddData(Guid sensorGroupId, Guid userId, SensorDetailsData data)
		{
			var sensorGroup = _context.SensorGroups.FirstOrDefault(sg => sg.Id == sensorGroupId);
			if (sensorGroup == null) throw new ArgumentException("Group not found");

			if (sensorGroup.UserId == userId) throw new UnauthorizedAccessException();

			JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(data.Body.ToString());
			List<JsonElement> listJsonElements = new();

			if (jsonElement.ValueKind == JsonValueKind.Array)
			{

				var jsonArray = jsonElement.EnumerateArray();
                var propertiesOfFirstElement = jsonArray.First().EnumerateObject().Select(p => p.Name).ToList();

                foreach (JsonElement item in jsonArray)
                {

                    var properties = item.EnumerateObject().Select(p => p.Name).ToList();

                    if (!properties.SequenceEqual(propertiesOfFirstElement))
                    {
						throw new ArgumentException("Properties for json are not the same");
                    }
					listJsonElements.Add(item);
                }
			}
			else if(jsonElement.ValueKind == JsonValueKind.Object) {
				listJsonElements.Add(jsonElement);
			}

            var sensorGroupDetails = await _sensorCollection.Find(p => p.SensorGroupId == sensorGroupId.ToString()).FirstOrDefaultAsync();
            List<SensorsDetails> listSensorsDetails = new();

			foreach(var element in listJsonElements) {
				if(sensorGroupDetails != null) {
                    var dynamicObject = BsonSerializer.Deserialize<ExpandoObject>(sensorGroupDetails?.Body);
                    if (!IsJsonOfType(element, dynamicObject))
                        throw new ArgumentException("Type for input not matching existing recors structure");
                }


				var sensorsDetails = new SensorsDetails()
				{
					Body = BsonDocument.Parse(element.ToString()),
					DateTime = DateTime.Now,
					SensorGroupId = sensorGroupId.ToString(),
				};

				listSensorsDetails.Add(sensorsDetails);
            }

			await _sensorCollection.InsertManyAsync(listSensorsDetails);
        }

		private bool IsJsonOfType(JsonElement jsonElement, dynamic customClass)
        {
			var dictionary = (IDictionary<string, object>)customClass;

			if (dictionary.Count != jsonElement.EnumerateObject().Count()) return false;

            foreach (var propertyName in dictionary.Keys)
            {
                if (!jsonElement.TryGetProperty(propertyName, out _))
                {
                    return false;
                }
            }

            return true;
        }
    }

}

