﻿using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Enumerations;
using HouseRentSystem.Core.Exceptions;
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
                   Address=h.Address,
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
                    .OrderBy(h => h.PricePerMonth),
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
                .ProjectToHouseServiceModel()
                //.Select(h => new HouseServiceModel()
                //{
                //    Id = h.Id,
                //    Address = h.Address,
                //    ImageUrl = h.ImageUrl,
                //    IsRented = h.RenterId !=null,
                //    PricePerMonth = h.PricePerMonth,
                //    Title = h.Title
                //})
                .ToListAsync();

            int totalHouses = await housesToShow.CountAsync();

            return new HouseQueryServiceModel()
            {
                Houses = houses,
                TotalHousesCount = totalHouses
            };
        }

        public async Task<IEnumerable<string>> AllCategoriesNamesAsync()
        {
            return await repository.AllReadOnly<Category>()
                 .Select(c => c.Name)
                 .Distinct()
                 .ToListAsync();
        }

        //6 My Houses methods

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int agentId)
        {
            return await repository.AllReadOnly<House>()
                .Where(h => h.AgentId == agentId)
                .ProjectToHouseServiceModel()
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId)
        {
            return await repository.AllReadOnly<House>()
                .Where(h => h.RenterId == userId)
                .ProjectToHouseServiceModel()
                .ToListAsync();
        }


        //7 Details

        public async Task<bool> ExistsAsync(int id)
        {
            return await repository.AllReadOnly<House>()
                .AnyAsync(h => h.Id == id);
        }

        public async Task<HouseDetailsServiceModel> HouseDetailsByIdAsync(int id)
        {
            return await repository.AllReadOnly<House>()
                .Where(h => h.Id == id)
                .Select(h => new HouseDetailsServiceModel()
                {
                    Id = h.Id,
                    Address = h.Address,
                    Agent = new Models.Agent.AgentServiceModel()
                    {
                        Email = h.Agent.User.Email,
                        PhoneNumber = h.Agent.PhoneNumber
                    },
                    Category = h.Category.Name,
                    Description = h.Description,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .FirstAsync();
        }

        //8 Edit
        public async Task EditAsync(int houseId, HouseFormModel model)
        {
            var house = await repository.GetByIdAsync<House>(houseId);

            if (house != null)
            {
                house.Address = model.Address;
                house.CategoryId = model.CategoryId;
                house.Description = model.Description;
                house.ImageUrl = model.ImageUrl;
                house.PricePerMonth = model.PricePerMonth;
                house.Title = model.Title;

                await repository.SaveChangesAsync();
            }
        }

        public async Task<bool> HasAgentWithIdAsync(int houseId, string userId)
        {
            return await repository.AllReadOnly<House>()
                .AnyAsync(h => h.Id == houseId && h.Agent.UserId == userId);
        }

        public async Task<HouseFormModel?> GetHouseFormModelByIdAsync(int id)
        {
            var house = await repository.AllReadOnly<House>()
                 .Where(h => h.Id == id)
                 .Select(h => new HouseFormModel()
                 {
                     Address = h.Address,
                     CategoryId = h.CategoryId,
                     Description = h.Description,
                     ImageUrl = h.ImageUrl,
                     PricePerMonth = h.PricePerMonth,
                     Title = h.Title
                 })
                 .FirstOrDefaultAsync();

            if (house != null)
            {
                house.Categories = await AllCategoriesAsync();
            }

            return house;
        }

        //8 Delete
        public async Task DeleteAsync(int houseId)
        {
            await repository.DeleteAsync<House>(houseId);
            await repository.SaveChangesAsync();
        }

        //9 Rent
        public async Task<bool> IsRentedAsync(int houseId)
        {
            //if the is an Id of renter the house is rented.

            bool result = false;
            var house = await repository.GetByIdAsync<House>(houseId);

            if (house != null)
            {
                result = house.RenterId != null;
            }


            //if the result is null we will return false!
            return result;
        }

        public async Task<bool> IsRentedByIUserWithIdAsync(int houseId, string userId)
        {
            bool result = false;
            var house = await repository.GetByIdAsync<House>(houseId);

            if (house != null)
            {
                result = house.RenterId == userId;
            }


            return result;
        }

        public async Task RentAsync(int houseId, string userId)
        {
            var house = await repository.GetByIdAsync<House>(houseId);

            if (house != null)
            {
                house.RenterId = userId;
                await repository.SaveChangesAsync();
            }
        }

        //10
        public async Task LeaveAsync(int houseid, string userId)
        {
            var house = await repository.GetByIdAsync<House>(houseid);

            if (house != null)
            {
                if (house.RenterId!= userId)
                {
                    throw new UnauthorizedActionException("The user is not the renter!");
                }
                house.RenterId = null;
                await repository.SaveChangesAsync();
            }
        }
    }
}
