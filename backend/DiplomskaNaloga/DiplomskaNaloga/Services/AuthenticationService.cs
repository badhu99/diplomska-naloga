using AutoMapper;
using Entity;
using DiplomskaNaloga.Models;
using DiplomskaNaloga.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;

namespace DiplomskaNaloga.Services
{
    public interface IAuthenticationService
    {
        public Task<UserDto> SignUp(UserRequest request);
        public Task<UserDto> SignIn(UserLogin request);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IMongoCollection<User> _collection;

        public AuthenticationService(
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
        public async Task<UserDto> SignIn(UserLogin request)
        {
            var filter = Builders<User>.Filter.Or(
                    Builders<User>.Filter.Eq("Username", request.Username)
                );
            var entityUser = _collection.Find(filter).FirstOrDefault();
            if (entityUser == null) throw new ArgumentException("Username or password incorrect!");

            if (_passwordService.Verify(request.Password, entityUser.Password, entityUser.PasswordHash) == false) throw new ArgumentException("Username or password incorrect!");
            if (entityUser.IsActive == false) throw new ArgumentException("User not yet activated");

            _jwtService.CreateRefreshToken(entityUser);


            var userDto = _mapper.Map<UserDto>(entityUser);
            userDto.AccessToken = _jwtService.CreateAccesToken(entityUser);
            return userDto;
        }

        public async Task<UserDto> SignUp(UserRequest request)
        {

            var filter = Builders<User>.Filter.Or(
                    Builders<User>.Filter.Eq("Username", request.Username),
                    Builders<User>.Filter.Eq("Email", request.Email)
                );
            var entityUser =  _collection.Find(filter).FirstOrDefault();
            if (entityUser != null)
            {
                throw new ArgumentException("Username or email address already exists!");
            }

            entityUser = _mapper.Map<User>(request);
            entityUser.Password = _passwordService.Hash(request.Password, out var salt);
            entityUser.PasswordHash = salt;
            entityUser.Id = Guid.NewGuid();
            entityUser.IsAdmin = false;
            entityUser.IsActive = false;

            await _collection.InsertOneAsync(entityUser);


            _jwtService.CreateRefreshToken(entityUser);

            var userDto = _mapper.Map<UserDto>(entityUser);

            userDto.AccessToken = _jwtService.CreateAccesToken(entityUser);

            return userDto;
        }
    }
}
