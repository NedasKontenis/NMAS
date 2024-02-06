using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.IllegalMigrantEntity
{
    public interface IIllegalMigrantEntityRepository
    {
        Task<int> CreateAsync(IllegalMigrantEntityDocument document);
    }
}
