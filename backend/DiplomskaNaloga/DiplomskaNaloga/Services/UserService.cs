using System;
using AutoMapper;
using DiplomskaNaloga.Models;
using DiplomskaNaloga.Settings;
using Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiplomskaNaloga.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task ChangeUserActive(Guid userId, bool isActive);
    }

	public class UserService:IUserService
	{

        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IMongoCollection<User> _collection;

        public UserService(
            IMapper mapper,
            IJwtService jwtService,
            IPasswordService passwordService,
            IOptions<MongoDbSettings> options)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _passwordService = passwordService;

            var settings = options.Value;

            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(settings.DatabaseName);

            _collection = mongoDb.GetCollection<User>(settings.CollectionUsersName);
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var entityUsers = await _collection.Find(_ => true).ToListAsync();

            return _mapper.Map<List<UserDto>>(entityUsers);
        }

        public async Task ChangeUserActive(Guid userId, bool isActive)
        {
            var filter = Builders<User>.Filter.Eq(c => c.Id, userId);
            var update = Builders<User>.Update
                .Set(c => c.IsActive, isActive);

            await _collection.UpdateOneAsync(filter, update);
        }
	}
}

