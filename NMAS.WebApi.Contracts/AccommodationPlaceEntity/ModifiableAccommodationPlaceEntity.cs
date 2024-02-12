using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Contracts.AccommodationPlaceEntity
{
    public abstract class ModifiableAccommodationPlaceEntity
    {
        /// <summary>
        ///     The id of worker
        /// </summary>
        /// <example>1</example>
        [Required]
        public int WorkerId { get; set; }

        /// <summary>
        ///     Name of accommodation place
        /// </summary>
        /// <example>Pabrades užsienieciu registracijos centras</example>
        [Required]
        public string PlaceName { get; set; }

        /// <summary>
        ///     Adress of accommodation place
        /// </summary>
        /// <example>Vilniaus g. 100, Pabrades m., LT-18177 Švencioniu r.</example>
        [Required]
        public string Adress { get; set; }

        /// <summary>
        ///     The capacity of accomodation place
        /// </summary>
        /// <example>1000</example>
        [Required]
        public int AccommodationCapacity { get; set; }

        /// <summary>
        ///     The used capacity of accomodation place
        /// </summary>
        /// <example>500</example>
        [Required]
        public int UsedAccommodationCapacity { get; set; }

        /// <summary>
        ///     The accommodation place company code
        /// </summary>
        /// <example>50005102662</example>
        public string CompanyCode { get; set; }

        /// <summary>
        ///     The contact phone of accommodation place
        /// </summary>
        /// <example>(8-387) 53401</example>
        public string ContactPhone { get; set; }
    }
}
