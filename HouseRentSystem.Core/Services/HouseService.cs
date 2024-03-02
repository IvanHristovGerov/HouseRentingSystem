using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Models.Home;
using HouseRentSystem.Infrastructure.Data.Common;
using HouseRentSystem.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseRentSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository repository;

        public HouseService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<HouseIndexServiceModel>> LastThreeHousesAsync()
        {
            return await repository
               .AllReadOnly<House>()
               .OrderByDescending(h => h.Id)
               .Take(3)
               .Select(h => new HouseIndexServiceModel()
               {
                   Id = h.Id,
                   ImageUrl = h.ImageUrl,
                   Title = h.Title
               })
               .ToListAsync();
        }
    }
}
