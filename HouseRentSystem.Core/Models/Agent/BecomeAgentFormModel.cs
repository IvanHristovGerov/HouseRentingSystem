using System.ComponentModel.DataAnnotations;
using static HouseRentSystem.Core.Constants.MessageConstants;
using static HouseRentSystem.Infrastructure.Constants.DataConstants;

namespace HouseRentSystem.Core.Models.Agent
{
    public class BecomeAgentFormModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(AgentPhoneMaxLength, MinimumLength = AgentPhoneMinLength, 
            ErrorMessage = LengthMessage)]
        [Display(Name ="Phone number")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
