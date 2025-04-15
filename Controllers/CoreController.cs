using TMODELOBASET_WebAPI_CS.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMODELOBASET_WebAPI_CS.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CoreController : ControllerBase
    {
        private readonly ICoreService coreService;

        public CoreController(ICoreService CoreService)
        {
            this.coreService = CoreService;
        }

        [HttpGet("{IDCargo}")]
        public ActionResult Index(String IDCargo)
        {
            return Ok(this.coreService.ExibirMenu(IDCargo));
        }
    }
}