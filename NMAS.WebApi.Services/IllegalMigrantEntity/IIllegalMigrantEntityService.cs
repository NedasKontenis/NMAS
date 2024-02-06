using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using System.Threading.Tasks;

namespace NMAS.WebApi.Services.IllegalMigrantEntity
{
    public interface IIllegalMigrantEntityService
    {
        Task<IllegalMigrantEntityCreated> InsertAsync(Client.IllegalMigrantEntity illegalMigrantEntity);
    }
}
