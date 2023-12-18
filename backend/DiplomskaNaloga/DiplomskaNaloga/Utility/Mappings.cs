using AutoMapper;
using DiplomskaNaloga.Models;

namespace DiplomskaNaloga.Utility
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<UserRequest, Entity.User>();
            CreateMap<Entity.User, UserDto>();

            CreateMap<Entity.SensorGroup, SensorGroupDto>();
            CreateMap<SensorGroupData, Entity.SensorGroup>();
        }
    }
}
