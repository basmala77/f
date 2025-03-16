using Azure.Core;
using DataAcess;
using Microsoft.AspNetCore.Mvc;
using IdentityManager.Services.ControllerService.IControllerService;
using Amazon.Util.Internal.PlatformServices;
using Microsoft.EntityFrameworkCore;
using IdentityManagerAPI.ControllerService.IControllerService;

namespace IdentityManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : Controller
    {
        private readonly IWorkerFacadeService _workerFacadeService;
        private readonly ApplicationDbContext _context;
        public ServiceRequestsController(ApplicationDbContext context,IWorkerFacadeService workerFacade )
        {
            _context = context;
            _workerFacadeService = workerFacade;
        }

        [HttpPost]
        public async Task<IActionResult> RequestService([FromBody] ServiceRequest request)
        {
            var factory = new ServiceFactory();
            var service = factory.CreateService(request.ServiceType);
            if (service == null)
                return BadRequest("Unsupported service");

            _context.ServiceRequests.Add(request);
            _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("AllService")]
        public async Task<IActionResult> GetAllService()
        {
            var c= _context.Workers
                           .Select(w => w.Specialty)
                           .Distinct()
                           .ToList();
            return Ok(c);
        }
        [HttpGet("workers")]
        public async Task<IActionResult> GetNearbyWorker([FromQuery] string category, [FromQuery] double userLat, [FromQuery] double userLon)
        {
            var workers = await _workerFacadeService.GetWorkersByCategoryWithNear(category, userLat, userLon);
            return Ok(workers);
        }
    }
}
