using lp_api.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = LpRoles.ADMIN)]
        public ActionResult<string> admin()
        {
            return LpRoles.ADMIN;
        }

        [HttpPost]
        [Authorize(Roles = LpRoles.USER)]
        public ActionResult<string> student()
        {
            return LpRoles.USER;
        }


    }
}
