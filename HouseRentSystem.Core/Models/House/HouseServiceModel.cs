using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static HouseRentSystem.Core.Constants.MessageConstants;
using static HouseRentSystem.Infrastructure.Constants.DataConstants;

namespace HouseRentSystem.Core.Models.House
{
    public class HouseServiceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseTitleMaxLength, MinimumLength = HouseTitleMinLength, ErrorMessage = LengthMessage)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseAddressMaxLength, MinimumLength = HouseAddressMinLength, ErrorMessage = LengthMessage)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; } = string.Empty;


        [Required(ErrorMessage = RequiredMessage)]
        [Range(typeof(decimal), HouseRentingPriceMin,
            HouseRentingPriceMax,
            ConvertValueInInvariantCulture = true,
            ErrorMessage = "Price per month must be a positive number and less then {2} leva")]
        [DisplayName("Price per month")]
        public decimal PricePerMonth { get; set; }

        [DisplayName("Is Rented")]
        public bool IsRented { get; set; }
    }
}