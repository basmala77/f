using Azure.Core;
using DataAcess;
using Microsoft.AspNetCore.Mvc;
using IdentityManager.Services.ControllerService.IControllerService;
using Amazon.Util.Internal.PlatformServices;
using Microsoft.EntityFrameworkCore;

namespace IdentityManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : Controller
    {

        private readonly ApplicationDbContext _context;
        public ServiceRequestsController(ApplicationDbContext context )
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RequestService([FromBody] ServiceRequest request)
        {
            var service = IdentityManager.Services.ControllerService.IControllerService.ServiceFactory.CreateService(request.ServiceType);
            if (service == null)
                return BadRequest("Unsupported service");

            _context.ServiceRequests.Add(request);
            _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllService()
        {
            var c= _context.Workers
                           .Select(w => w.Specialty)
                           .Distinct()
                           .ToList();
            return Ok(c);
        }
    }
}
