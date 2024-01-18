using System.Dynamic;
using System.Text.Json;
using Entity;
using DiplomskaNaloga.Models;
using DiplomskaNaloga.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Globalization;

namespace DiplomskaNaloga.Services
{
    public interface ISensorDataService {
		Task AddData(Guid sensorGroupId, SensorDetailsData data);
		Task<SensorDetailsResponse> GetData(Guid? userId, Guid sensorGroupId, DateTime? startDate = null, DateTime? EndDate = null);
		Task<bool> VerifyApiKey(Guid sensorGroupId, string apiKey);

    }
    public class SensorDataService : ISensorDataService
	{

		private readonly IMongoCollection<SensorsDetails> _sensorDetailsCollection;
		private readonly IMongoCollection<SensorGroup> _sensorGroupCollection;

        public SensorDataService(IOptions<MongoDbSettings> options)
		{
			var settings = options.Value;

			var mongoClient = new MongoClient(settings.ConnectionString);
			var mongoDb = mongoClient.GetDatabase(settings.DatabaseName);

            _sensorDetailsCollection = mongoDb.GetCollection<SensorsDetails>(settings.CollectionDetailsName);
            _sensorGroupCollection = mongoDb.GetCollection<SensorGroup>(settings.CollectionGroupName);
        }

		public async Task<SensorDetailsResponse> GetData(Guid? userId, Guid sensorGroupId, DateTime? startDate = null, DateTime? endDate = null) {
			var sensorGroup = await _sensorGroupCollection.Find(sgc => sgc.Id == sensorGroupId).FirstOrDefaultAsync();
			if (sensorGroup == null) throw new UnauthorizedAccessException();

			var c = _sensorDetailsCollection
				.AsQueryable()
				.Where(sg => sg.SensorGroupId == sensorGroupId.ToString())
				.Select(sg =>
					sg.Body
				).ToList();

			SensorDetailsResponse response = new()
			{
				Name = sensorGroup.Name,
				Content = new(),
				XAxis = sensorGroup.ColumnX ?? "",
				YAxis = sensorGroup.ColumnY ?? "",
			};

			SensorDetailsContent responseContent = new()
			{
				Name = sensorGroup.Name,
				Series = new(),
			};

			foreach (var b in c)
			{
                DateTime dataDateTime = DateTime.ParseExact(b[sensorGroup.ColumnX].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
				if (startDate.HasValue)
				{
					if(dataDateTime < startDate)
					{
						continue;
					}
				}
				if (endDate.HasValue)
				{
					if(dataDateTime >= endDate)
					{
						continue;
					}
				}

                var data = new
				{
					Name = b[sensorGroup.ColumnX].ToString(),
					Value = b[sensorGroup.ColumnY].ToString(),
				};
				responseContent.Series.Add(data);
			}

			response.Content.Add(responseContent);
			response.UserId = sensorGroup.UserId;
			if(userId.HasValue && userId.Value == sensorGroup.UserId)
			{
				response.SensorHash = sensorGroup.Hash;
			}

			return response;
        }


		public async Task AddData(Guid sensorGroupId, SensorDetailsData data)
		{
			var sensorGroup = await _sensorGroupCollection.Find(sgc => sgc.Id == sensorGroupId).FirstOrDefaultAsync();
			//if (sensorGroup.Hash != data.SensorHash) throw new Exception("Invalid hash");

			if (sensorGroup == null) throw new ArgumentException("Group not found");

			//if (sensorGroup.UserId != userId && role != "Admin") throw new UnauthorizedAccessException();

			JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(data.Body.ToString());
			List<JsonElement> listJsonElements = new();

			if (jsonElement.ValueKind == JsonValueKind.Array)
			{
				var jsonArray = jsonElement.EnumerateArray();
				var propertiesOfFirstElement = jsonArray.First().EnumerateObject().Select(p => p.Name).ToList();

				foreach (JsonElement item in jsonArray)
				{
					var properties = item.EnumerateObject().Select(p => p.Name).ToList();
					foreach (var p in properties)
					{
						if (p != sensorGroup.ColumnX && p != sensorGroup.ColumnY)
						{
							throw new ArgumentException($"Property {p} not named {sensorGroup.ColumnX} or {sensorGroup.ColumnY}");
						}
					}

					if (!properties.SequenceEqual(propertiesOfFirstElement))
					{
						throw new ArgumentException("Properties for json are not the same");
					}
					listJsonElements.Add(item);
				}
			}
			else if (jsonElement.ValueKind == JsonValueKind.Object)
			{
				listJsonElements.Add(jsonElement);
			}

			var sensorGroupDetails = await _sensorDetailsCollection.Find(p => p.SensorGroupId == sensorGroupId.ToString()).FirstOrDefaultAsync();
			List<SensorsDetails> listSensorsDetails = new();

			foreach (var element in listJsonElements)
			{
				if (sensorGroupDetails != null)
				{
					var dynamicObject = BsonSerializer.Deserialize<ExpandoObject>(sensorGroupDetails?.Body);
					if (!IsJsonOfType(element, dynamicObject))
						throw new ArgumentException("Type for input not matching existing records structure");
				}


				var sensorsDetails = new SensorsDetails()
				{
					Body = BsonDocument.Parse(element.ToString()),
					DateTime = DateTime.Now,
					SensorGroupId = sensorGroupId.ToString(),
				};

				listSensorsDetails.Add(sensorsDetails);
			}

			await _sensorDetailsCollection.InsertManyAsync(listSensorsDetails);
		}

        public async Task<bool> VerifyApiKey(Guid sensorGroupId, string apiKey)
		{
			var sensorGroup = await _sensorGroupCollection.Find(sgc => sgc.Id == sensorGroupId).FirstOrDefaultAsync();

			if (sensorGroup == null) return false;

			if (sensorGroup.Hash == apiKey) return true;

            return false;
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

