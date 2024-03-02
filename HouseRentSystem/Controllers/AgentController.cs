using HouseRentSystem.Core.Contracts;
using HouseRentSystem.Core.Models.Agent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentSystem.Controllers
{
    
    public class AgentController : BaseController
    {
        private readonly IAgentService agentService;

        public AgentController(IAgentService _agentService)
        {
            agentService = _agentService;
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            if (await agentService.ExistsByIdAsync(User.Id()))
            {
                return BadRequest();
            }

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
