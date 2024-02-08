namespace NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity
{
    public class AccommodationPlaceEntityDocument
    {
        public int ID { get; set; }
        public int WorkerID { get; set; }
        public string PlaceName { get; set; }
        public string Adress { get; set; }
        public int AccommodationCapacity { get; set; }
        public int UsedAccommodationCapacity { get; set; }
        public string CompanyCode { get; set; }
        public string ContactPhone { get; set; }
    }
}
