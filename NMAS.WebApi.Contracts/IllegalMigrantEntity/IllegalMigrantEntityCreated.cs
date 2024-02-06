using NMAS.WebApi.Contracts.Response;

namespace NMAS.WebApi.Contracts.IllegalMigrantEntity
{
    public class IllegalMigrantEntityCreated : BaseResponse
    {
        public int Id { get; set; }

        public IllegalMigrantEntityCreated(int id, string message = "Illegal migrant created successfully.")
            : base(true, message)
        {
            Id = id;
        }
    }
}
