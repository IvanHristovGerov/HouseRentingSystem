using HouseRentSystem.Core.Enumerations;
using System.ComponentModel;
using System.Globalization;

namespace HouseRentSystem.Core.Models.House
{
    public class AllHousesQueryModel
    {
        public  int HousesPerPage { get; } = 3;

        public string Category { get; set; } = string.Empty;

        [DisplayName("Search by text")]
        public string SearchTerm { get; set; } = string.Empty;

        public HouseSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalHousesCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = null!;

        public IEnumerable<HouseServiceModel> Houses { get; set; } = new List<HouseServiceModel>();





    }
}
