using NMAS.WebApi.Contracts.Response;

namespace NMAS.WebApi.Contracts.AccomodationPlaceEntity
{
    public class AccomodationPlaceEntityCreated : BaseResponse
    {
        public int Id { get; set; }

        public AccomodationPlaceEntityCreated(int id, string message = "Accommodation place created successfully.")
            : base(true, message)
        {
            Id = id;
        }
    }
}
