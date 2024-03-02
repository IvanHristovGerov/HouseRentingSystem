using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Models.Home;
using HouseRentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HouseRentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHouseService houseService;

        public HomeController(
            ILogger<HomeController> logger,
            IHouseService _houseService)
        {
            _logger = logger;
            houseService = _houseService;
        }

        public async Task< IActionResult> Index()
        {
            var model = await houseService.LastThreeHouses();

            return View(model);
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
