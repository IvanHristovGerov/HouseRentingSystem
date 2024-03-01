using HouseRentSystem.Core.Models.Agent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentSystem.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Become()
        {
            var model = new BecomeAgentFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeAgentFormModel agent)
        {
            return RedirectToAction(nameof(HouseController.All), "House");
        }
    }
}
