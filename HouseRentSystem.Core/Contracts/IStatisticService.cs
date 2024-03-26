using HouseRentSystem.Core.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentSystem.Core.Contracts
{
    public interface IStatisticService
    {
        Task<StatisticServiceModel> TotalAsync();
    }
}
