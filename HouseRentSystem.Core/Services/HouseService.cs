using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Models.Home;
using HouseRentSystem.Core.Models.House;
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


        //1
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

        //2
        public async Task<IEnumerable<HouseCategoryServiceModel>> AllCategoriesAsync()
        {
            return await repository.AllReadOnly<Category>()
                .Select(c => new HouseCategoryServiceModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        //3
        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await repository.AllReadOnly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

        //4
        public async Task<int> CreateAsync(HouseFormModel model, int agentId)
        {
            House house = new House()
            {
                Address = model.Address,
                AgentId = agentId,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                Title = model.Title
            };

            await repository.AddAsync(house);
            await repository.SaveChangesAsync();

            return house.Id;
        }

        //5
    }
}
