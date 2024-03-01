using HouseRentSystem.Core.Models.Home;

namespace HouseRentSystem.Core.Contracts
{
    public interface IHouseService
    {
        Task<IEnumerable<HouseIndexServiceModel>> LastThreeHouses();
    }
}
