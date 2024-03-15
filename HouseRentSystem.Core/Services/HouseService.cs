using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Enumerations;
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


        //5 All Houses methods
        public async Task<HouseQueryServiceModel> AllAsync(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1, 
            int housesPerPage = 1)
        {
            var housesToShow = repository.AllReadOnly<House>();

            if (category != null)
            {
                housesToShow = housesToShow
                    .Where(h => h.Category.Name == category);
            }

            if (searchTerm != null)
            {
                string normalizedSearchTerm = searchTerm.ToLower();

                housesToShow = housesToShow
                    .Where(h => (h.Title.ToLower().Contains(normalizedSearchTerm) ||
                                 h.Address.ToLower().Contains(normalizedSearchTerm) ||
                                 h.Description.ToLower().Contains(normalizedSearchTerm)));
            }

            housesToShow = sorting switch
            {
                HouseSorting.Price => housesToShow
                    .OrderByDescending(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => housesToShow
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.Id),
                _ => housesToShow
                    .OrderByDescending(h => h.Id)

                //in switch expression _=> means default value
            };

            //pages
            var houses = await housesToShow
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId !=null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();

            int totalHouses = await housesToShow.CountAsync();

            return new HouseQueryServiceModel()
            {
                Houses = houses,
                TotalHousesCount = totalHouses
            };
        }

        public Task<IEnumerable<string>> AllCategoriesNamesAsync()
        {
            throw new NotImplementedException();
        }


    }
}
