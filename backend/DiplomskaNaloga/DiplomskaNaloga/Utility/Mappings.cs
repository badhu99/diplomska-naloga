using AutoMapper;
using DiplomskaNaloga.Models;

namespace DiplomskaNaloga.Utility
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<UserRequest, Data.Entity.User>();
            CreateMap<Data.Entity.User, UserDto>();
        }
    }
}
