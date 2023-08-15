using AutoMapper;
using Data;
using Data.Entity;
using DiplomskaNaloga.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly databaseContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public AuthenticationService(databaseContext context, IMapper mapper, IJwtService jwtService, IPasswordService passwordService)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }
        public async Task<UserDto> SignIn(UserLogin request)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (entity == null) throw new ArgumentException("Username or password incorrect!");

            if (_passwordService.Verify(request.Password, entity.Password, entity.PasswordHash) == false) throw new ArgumentException("Username or password incorrect!");


            _jwtService.CreateRefreshToken(entity);

            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(entity);

            userDto.AccessToken = _jwtService.CreateAccesToken(userDto);

            return userDto;
        }

        public async Task<UserDto> SignUp(UserRequest request)
        {
            var entityUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (entityUser != null)
            {
                throw new ArgumentException("Username or email address already exists!");
            }

            entityUser = _mapper.Map<User>(request);
            entityUser.Password = _passwordService.Hash(request.Password, out var salt);
            entityUser.PasswordHash = salt;

            await _context.Users.AddAsync(entityUser);
            await _context.SaveChangesAsync();


            _jwtService.CreateRefreshToken(entityUser);

            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(entityUser);

            userDto.AccessToken = _jwtService.CreateAccesToken(userDto);

            return userDto;
        }
    }
}
