using HouseRentSystem.Core.Models.House;
using HouseRentSystem.Infrastructure.Data.Models;

namespace System.Linq
{ 
    public static class IQueryableHouseExtension
    {
        public static IQueryable<HouseServiceModel> ProjectToHouseServiceModel(this IQueryable<House> houses)
        {
            return houses
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    Title = h.Title,
                });
        }
    }
}
