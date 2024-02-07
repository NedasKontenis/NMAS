using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.IllegalMigrantEntity;
using NMAS.WebApi.Services.Extensions;
using System.Threading.Tasks;

namespace NMAS.WebApi.Services.IllegalMigrantEntity
{
    public class IllegalMigrantEntityService : IIllegalMigrantEntityService
    {
        private readonly IIllegalMigrantEntityRepository _illegalMigrantEntityRepository;

        public IllegalMigrantEntityService(IIllegalMigrantEntityRepository illegalMigrantEntityRepository)
        {
            _illegalMigrantEntityRepository = illegalMigrantEntityRepository;
        }

        public async Task<IllegalMigrantEntityCreated> CreateAsync(CreateIllegalMigrantEntity createIllegalMigrantEntity)
        {
            var document = createIllegalMigrantEntity.Map();
            var createdIllegalMigrantEntityId = await _illegalMigrantEntityRepository.CreateAsync(document);

            return new IllegalMigrantEntityCreated
            {
                Id = createdIllegalMigrantEntityId
            };
        }

        public async Task<Contracts.IllegalMigrantEntity.IllegalMigrantEntity> GetAsync(int id)
        {
            var illegalMigrantEntityDocument = await _illegalMigrantEntityRepository.GetAsync(id);

            if (illegalMigrantEntityDocument == null)
            {
                throw new ResourceNotFoundException("Illegal migrant entity not found");
            }

            return illegalMigrantEntityDocument.Map();
        }

        public async Task UpdateAsync(int id, UpdateIllegalMigrantEntity updateIllegalMigrantEntity)
        {
            var illegalMigrantEntityDocument = await _illegalMigrantEntityRepository.GetAsync(id);

            if (illegalMigrantEntityDocument == null)
            {
                throw new ResourceNotFoundException("Illegal migrant entity not found");
            }

            await _illegalMigrantEntityRepository.UpdateAsync(id, updateIllegalMigrantEntity.Map());
        }

        public async Task DeleteAsync(int id)
        {
            var illegalMigrantEntityDocument = await _illegalMigrantEntityRepository.GetAsync(id);

            if (illegalMigrantEntityDocument == null)
            {
                throw new ResourceNotFoundException("Illegal migrant entity not found");
            }

            await _illegalMigrantEntityRepository.DeleteAsync(id);
        }
    }
}
