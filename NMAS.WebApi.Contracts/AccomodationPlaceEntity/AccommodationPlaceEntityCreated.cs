using NMAS.WebApi.Contracts.Response;

namespace NMAS.WebApi.Contracts.AccommodationPlaceEntity
{
    public class AccommodationPlaceEntityCreated : BaseResponse
    {
        public int Id { get; set; }

        public AccommodationPlaceEntityCreated(int id, string message = "Accommodation place created successfully.")
            : base(true, message)
        {
            Id = id;
        }
    }
}
