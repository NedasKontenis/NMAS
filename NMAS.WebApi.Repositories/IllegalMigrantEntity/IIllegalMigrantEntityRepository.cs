using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.IllegalMigrantEntity
{
    public interface IIllegalMigrantEntityRepository
    {
        Task<int> CreateAsync(IllegalMigrantEntityDocument document);

        Task<IllegalMigrantEntityDocument> GetAsync(int id);

        Task UpdateAsync(int id, IllegalMigrantEntityDocument document);

        Task DeleteAsync(int id);

        Task<IEnumerable<IllegalMigrantEntityDocument>> GetAllAsync();
    }
}
