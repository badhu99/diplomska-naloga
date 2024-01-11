using System;
using DiplomskaNaloga.Models;
using DipslomskaNaloga.Utility.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DiplomskaNaloga.Settings;
using System.Data;
using System.Text;

namespace DiplomskaNaloga.Services
{
    public interface ISensorService
    {
        Task<Pagination<SensorGroupDto>> GetPagination(Guid? userId, int pageNumber, int pageSize, bool OrderDesc, EnumSensorGroup orderBy);
        Task<Guid> AddNewSensorGroup(Guid userId, SensorGroupData data);
        Task DeleteSensorGroup(Guid userId, Guid id, string role);
        Task UpdateSensorGroup(Guid userId, Guid id, SensorGroupData data, string role);
    }

    public class SensorService : ISensorService
    {
        private readonly AutoMapper.IConfigurationProvider _config;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<SensorsDetails> _sensorDetailsCollection;
        private readonly IMongoCollection<SensorGroup> _sensorGroupCollection;


        public SensorService(
            AutoMapper.IConfigurationProvider config,
            IMapper mapper,
            IOptions<MongoDbSettings> options)
        {
            
            _config = config;
            _mapper = mapper;
            var settings = options.Value;

            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(settings.DatabaseName);

            _sensorDetailsCollection = mongoDb.GetCollection<SensorsDetails>(settings.CollectionDetailsName);
            _sensorGroupCollection = mongoDb.GetCollection<SensorGroup>(settings.CollectionGroupName);
        }

        public async Task<Guid> AddNewSensorGroup(Guid userId, SensorGroupData data)
        {
            var entity = await _sensorGroupCollection.Find(sgc => sgc.Name == data.Name).FirstOrDefaultAsync();

            if (entity != null) throw new ArgumentOutOfRangeException("Already exists");

            entity = new()
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                UserId = userId,
                ColumnX = data.ColumnX,
                ColumnY = data.ColumnY,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Hash = GenerateRandomString(16),
                Description = data.Description,
                Lat = data.Lat,
                Long = data.Long,
            };

            await _sensorGroupCollection.InsertOneAsync(entity);
            return entity.Id;
        }

        public async Task DeleteSensorGroup(Guid userId, Guid id, string role = "")
        {
            var entity = await _sensorGroupCollection.Find(sg => sg.Id == id).FirstOrDefaultAsync();
            if (entity == null) throw new ArgumentNullException("Not found");

            if (entity.UserId != userId && role != "Admin") throw new UnauthorizedAccessException();

            await _sensorGroupCollection.DeleteOneAsync(sga => sga.Id == id);
        }

        public async Task<Pagination<SensorGroupDto>> GetPagination(Guid? userId, int pageNumber, int pageSize, bool orderDesc, EnumSensorGroup orderBy)
        {
            var entities = _sensorGroupCollection.Find(_ => true).ToEnumerable();

            switch (orderBy)
            {
                case EnumSensorGroup.Name:
                    if (orderDesc)
                    {
                        entities = entities.OrderByDescending(e => e.Name);
                    }
                    else
                    {
                        entities = entities.OrderBy(e => e.Name);
                    }
                    break;
                case EnumSensorGroup.CreatedAt:
                    if (orderDesc)
                    {
                        entities = entities.OrderByDescending(e => e.CreatedAt);
                    }
                    else
                    {
                        entities = entities.OrderBy(e => e.CreatedAt);
                    }
                    break;
                case EnumSensorGroup.UpdatedAt:
                    if (orderDesc)
                    {
                        entities = entities.OrderByDescending(e => e.UpdatedAt);
                    }
                    else
                    {
                        entities = entities.OrderBy(e => e.UpdatedAt);
                    }
                    break;
            }

            List<SensorGroupDto> returnValues = new();
            foreach (var entity in entities.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize))
            {
                var returnValue = _mapper.Map<SensorGroupDto>(entity);
                if (userId.HasValue && userId.Value == entity.UserId)
                {
                    returnValue.SensorHash = entity.Hash;
                }
                returnValues.Add(returnValue);
            }


            return new Pagination<SensorGroupDto>
            {
                Count = entities.Count(),
                Page = pageNumber,
                Size = pageSize,
                Items = returnValues,
            };
        }

        public async Task UpdateSensorGroup(Guid userId, Guid id, SensorGroupData data, string role = "")
        {
            var entity = await _sensorGroupCollection.Find(sg => sg.Id == id).FirstOrDefaultAsync();

            if (entity == null) throw new ArgumentNullException("Not found");

            if (entity.UserId != userId && role != "Admin") throw new UnauthorizedAccessException();

            var filter = Builders<SensorGroup>.Filter.Eq(s => s.Id, id);
            var update = Builders<SensorGroup>.Update
                .Set(sg => sg.Name, data.Name)
                .Set(sg => sg.UpdatedAt, DateTime.Now);

            await _sensorGroupCollection.UpdateOneAsync(filter, update);            
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder randomStringBuilder = new StringBuilder(length);

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                randomStringBuilder.Append(chars[index]);
            }

            return randomStringBuilder.ToString();
        }
    }
}

