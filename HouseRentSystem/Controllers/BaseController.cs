using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentSystem.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        
    }
}
