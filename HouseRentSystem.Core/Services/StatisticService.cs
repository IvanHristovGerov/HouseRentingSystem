using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Models.Statistics;
using HouseRentSystem.Infrastructure.Data.Common;
using HouseRentSystem.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentSystem.Core.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IRepository repository;

        public StatisticService(IRepository _repository)
        {

            repository = _repository;
        }
        public async Task<StatisticServiceModel> TotalAsync()
        {
            int totalHouses = await repository.AllReadOnly<House>()
                 .CountAsync();

            int totalRents = await repository.AllReadOnly<House>()
                .Where(h => h.RenterId != null)
                .CountAsync();

            return new StatisticServiceModel()
            {
                TotalHouses = totalHouses,
                TotalRents = totalRents
            };
        }
    }
}
