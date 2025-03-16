using DataAcess;
using IdentityManager.Services.ControllerService;
using IdentityManager.Services.ControllerService.IControllerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace IdentityManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGeolocationService _geoService;

        public LocationController(IGeolocationService geoService,ApplicationDbContext context)
        {
            _geoService = geoService;
            _context = context; 
        }


        [HttpPost("reverse-geocode")]
        public async Task<IActionResult> GetLocation([FromBody] LocationRequest request)
        {
            string address = await _geoService.GetAddressFromCoordinates(request.Latitude, request.Longitude);
            return Ok(new { Address = address });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAction()
        {
            var locations = await _context.Workers
            .Select(w => w.Location).AsTracking()
            .ToListAsync();

            return Ok(locations);
        }
    }
}
