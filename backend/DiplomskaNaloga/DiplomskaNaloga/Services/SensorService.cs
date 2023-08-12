using System;
using Data;
using DiplomskaNaloga.Models;
using DipslomskaNaloga.Utility.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace DiplomskaNaloga.Services
{
    public interface ISensorService
    {
        Task<Pagination<SensorGroupDto>> GetPagination(Guid userId, int pageNumber, int pageSize, bool OrderDesc, EnumSensorGroup orderBy);
        Task<Guid> AddNewSensorGroup(Guid userId, SensorGroupData data);
        Task DeleteSensorGroup(Guid userId, Guid id);
        Task UpdateSensorGroup(Guid userId, Guid id, SensorGroupData data);
    }

    public class SensorService : ISensorService
    {
        private readonly databaseContext _context;
        private readonly AutoMapper.IConfigurationProvider _config;
        private readonly IMapper _mapper;

        public SensorService(databaseContext databaseContext, AutoMapper.IConfigurationProvider config, IMapper mapper)
        {
            _context = databaseContext;
            _config = config;
            _mapper = mapper;
        }

        public async Task<Guid> AddNewSensorGroup(Guid userId, SensorGroupData data)
        {
            var entity = await _context.SensorGroups.FirstOrDefaultAsync(sg => sg.Name == data.Name);

            if (entity != null) throw new ArgumentOutOfRangeException("Already exists");

            entity = new()
            {
                Name = data.Name,
                UserId = userId,
            };

            await _context.SensorGroups.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task DeleteSensorGroup(Guid userId, Guid id)
        {
            var entity = await _context.SensorGroups.FirstOrDefaultAsync(sg => sg.UserId == userId && sg.Id == id);

            if (entity == null) throw new ArgumentNullException("Not found");

            _context.SensorGroups.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Pagination<SensorGroupDto>> GetPagination(Guid userId, int pageNumber, int pageSize, bool orderDesc, EnumSensorGroup orderBy)
        {
            var entities = _context.SensorGroups
                .AsNoTrackingWithIdentityResolution()
                .Where(sg => sg.UserId == userId);


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




            return new Pagination<SensorGroupDto>
            {
                Count = entities.Count(),
                Page = pageNumber,
                Size = pageSize,
                Items = await entities
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<SensorGroupDto>(_config)
                    .ToListAsync(),
            };
        }

        public async Task UpdateSensorGroup(Guid userId, Guid id, SensorGroupData data)
        {
            var entity = await _context.SensorGroups.FirstOrDefaultAsync(sg => sg.UserId == userId && sg.Id == id);

            if (entity == null) throw new ArgumentNullException("Not found");

            entity.Name = data.Name;
            entity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();            
        }
    }
}

