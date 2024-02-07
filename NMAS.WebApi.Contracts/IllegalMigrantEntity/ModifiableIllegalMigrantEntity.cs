using System;
using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Contracts.IllegalMigrantEntity
{
    public abstract class ModifiableIllegalMigrantEntity
    {
        /// <summary>
        ///     The id of accomodation place
        /// </summary>
        /// <example>1</example>
        public int? AccommodationPlaceID { get; set; }

        /// <summary>
        ///     Personal identity code
        /// </summary>
        /// <example>50005102662</example>
        [Required]
        public string PersonalIdentityCode { get; set; }

        /// <summary>
        ///     First name of illegal migrant
        /// </summary>
        /// <example>Amar</example>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        ///     Middle name of illegal migrant
        /// </summary>
        /// <example>Abdu</example>
        public string MiddleName { get; set; }

        /// <summary>
        ///     Last name of illegal migrant
        /// </summary>
        /// <example>Ali</example>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        ///     Gender of illegal migrant
        /// </summary>
        /// <example>Male</example>
        [Required]
        public string Gender { get; set; }

        /// <summary>
        ///     Date of birth of illegal migrant
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        ///     Origin country of illegal migrant
        /// </summary>
        /// <example>Iran</example>
        [Required]
        public string OriginCountry { get; set; }

        /// <summary>
        ///     Religion of illegal migrant
        /// </summary>
        /// <example>Sunni</example>
        [Required]
        public string Religion { get; set; }
    }
}
