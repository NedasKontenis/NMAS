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

        public async Task<IllegalMigrantEntityCreated> InsertAsync(Client.IllegalMigrantEntity illegalMigrantEntity)
        {
            var document = illegalMigrantEntity.Map();
            await _illegalMigrantEntityRepository.CreateAsync(document);

            return new IllegalMigrantEntityCreated();
        }
    }
}
