using HouseRentSystem.Attributes;
using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Exceptions;
using HouseRentSystem.Core.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace HouseRentSystem.Controllers
{

    public class HouseController : BaseController
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;

        private readonly ILogger logger;

        public HouseController(
            IHouseService _houseService,
            IAgentService _agentService,
            ILogger<HouseController> _logger)

        {
            houseService = _houseService;
            agentService = _agentService;
            logger = _logger;
        }

        //1
        [HttpGet]
        [MustBeAgent]
        public async Task<IActionResult> Add()
        {

            var model = new HouseFormModel()
            {
                Categories = await houseService.AllCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        [MustBeAgent]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            if (await houseService.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = await houseService.AllCategoriesAsync();

                return View(model);
            }

            int? agentId = await agentService.GetAgentIdAsync(User.Id());

            int newHouseId = await houseService.CreateAsync(model, agentId ?? 0);

            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        //2
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel query)
        {
            var model = await houseService.AllAsync(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.HousesPerPage);

            query.TotalHousesCount = model.TotalHousesCount;
            query.Houses = model.Houses;

            query.Categories = await houseService.AllCategoriesNamesAsync();

            return View(query);
        }


        //3
        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = User.Id();
            IEnumerable<HouseServiceModel> model;

            if (await agentService.ExistsByIdAsync(userId))
            {
                var agentId = await agentService.GetAgentIdAsync(userId);
                model = await houseService.AllHousesByAgentId(agentId ?? 0);
            }
            else
            {
                //if it's not an agent
                model = await houseService.AllHousesByUserId(userId);
            }

            return View(model);
        }


        //4
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (await houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            var model = await houseService.HouseDetailsByIdAsync(id);

            return View(model);
        }


        //5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            if (await houseService.HasAgentWithIdAsync(id, User.Id()) == false)
            {
                return Unauthorized();
            }

            var model = await houseService.GetHouseFormModelByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseFormModel model)
        {
            if (await houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            if (await houseService.HasAgentWithIdAsync(id, User.Id()) == false)
            {
                return Unauthorized();
            }

            if (await houseService.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = await houseService.AllCategoriesAsync();

                return View(model);
            }

            await houseService.EditAsync(id, model);



            return RedirectToAction(nameof(Details), new { id });
        }


        //6
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {


            if (await houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }
            if (await houseService.HasAgentWithIdAsync(id, User.Id()) == false)
            {
                return Unauthorized();
            }

            var house = await houseService.HouseDetailsByIdAsync(id);

            var model = new HouseDetailsViewModel()
            {
                Id = id,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                Title = house.Title
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {

            if (await houseService.ExistsAsync(model.Id) == false)
            {
                return BadRequest();
            }
            if (await houseService.HasAgentWithIdAsync(model.Id, User.Id()) == false)
            {
                return Unauthorized();
            }

            await houseService.DeleteAsync(model.Id);

            return RedirectToAction(nameof(All));
        }


        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if (await houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            if (await agentService.ExistsByIdAsync(User.Id()))
            {
                return Unauthorized();
            }

            if (await houseService.IsRentedAsync(id))
            {
                return BadRequest();
            }

            await houseService.RentAsync(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if (await houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }
            try
            {
                await houseService.LeaveAsync(id, User.Id());
            }
            catch (UnauthorizedActionException uae)
            {

                logger.LogError(uae, "HouseController/Leave");

                return Unauthorized();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
