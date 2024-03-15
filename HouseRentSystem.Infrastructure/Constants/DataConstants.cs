using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentSystem.Infrastructure.Constants
{
    public static class DataConstants
    {
        public const int CategoryNameMaxLength = 50;

        public const int HouseTitleMaxLength = 50;
        public const int HouseTitleMinLength = 10;

        public const int HouseAddressMaxLength = 150;
        public const int HouseAddressMinLength = 30;

        public const int HouseDescriptionMaxLength = 500;
        public const int HouseDescriptionMinLength = 50;

        public const string HouseRentingPriceMin = "0";
        public const string HouseRentingPriceMax = "2000";

        public const int AgentPhoneMaxLength = 15;
        public const int AgentPhoneMinLength = 7;

    }
}
