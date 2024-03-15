using System.ComponentModel.DataAnnotations;
using static HouseRentSystem.Core.Constants.MessageConstants;
using static HouseRentSystem.Infrastructure.Constants.DataConstants;

namespace HouseRentSystem.Core.Models.House
{
    public class HouseFormModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseTitleMaxLength, MinimumLength = HouseTitleMinLength, ErrorMessage = LengthMessage)]

        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseAddressMaxLength, MinimumLength = HouseAddressMinLength, ErrorMessage = LengthMessage)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseDescriptionMaxLength, MinimumLength = HouseDescriptionMinLength, ErrorMessage = LengthMessage)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Range(typeof(decimal),HouseRentingPriceMin, 
            HouseRentingPriceMax,
            ConvertValueInInvariantCulture = true,
            ErrorMessage = "Price per month must be a positive number and less then {2} leva")]
        [Display(Name = "Price per Month")]
        public decimal PricePerMonth  { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<HouseCategoryServiceModel> Categories { get; set; } = new List<HouseCategoryServiceModel>();
    }
}
