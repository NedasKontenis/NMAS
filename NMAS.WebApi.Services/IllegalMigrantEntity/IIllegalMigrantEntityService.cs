﻿using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMAS.WebApi.Services.IllegalMigrantEntity
{
    public interface IIllegalMigrantEntityService
    {
        Task<IllegalMigrantEntityCreated> CreateAsync(CreateIllegalMigrantEntity createIllegalMigrantEntity);

        Task<Contracts.IllegalMigrantEntity.IllegalMigrantEntity> GetAsync(int id);

        Task UpdateAsync(int id, UpdateIllegalMigrantEntity updateIllegalMigrantEntity);

        Task DeleteAsync(int id);

        Task AssignAsync(int id);

        Task<IEnumerable<Contracts.IllegalMigrantEntity.IllegalMigrantEntity>> ListAsync(FilterIllegalMigrantEntity filter, FilterIllegalMigrantEntityPagination pagination);
    }
}