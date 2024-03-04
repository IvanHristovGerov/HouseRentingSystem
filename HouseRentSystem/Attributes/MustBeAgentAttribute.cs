using HouseRentSystem.Core.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HouseRentSystem.Controllers;

namespace HouseRentSystem.Attributes
{
    public class MustBeAgentAttribute:ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            //IoC
            IAgentService? agentService = context.HttpContext.RequestServices.GetService<IAgentService>();

            if (agentService == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (agentService != null &&
                agentService.ExistsByIdAsync(context.HttpContext.User.Id()).Result == false)
            {
                context.Result = new RedirectToActionResult(nameof(AgentController.Become), "Agent", null);
            }
        }    
    }
}
