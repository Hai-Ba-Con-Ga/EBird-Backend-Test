﻿using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IBirdTypeRepository : IGenericRepository<BirdTypeEntity>
    {
        Task<BirdTypeEntity> DeleteSoftAsync(string birdTypeCode);

        Task<BirdTypeEntity> GetBirdTypeByCode(string birdTypeCode);
        bool IsExistBirdTypeCode(string birdTypeCode);

        Task<List<BirdTypeEntity>> GetAllBirdTypeActiveAsync();

        Task<BirdTypeEntity> GetBirdTypeActiveAsync(Guid id);
    }
}
