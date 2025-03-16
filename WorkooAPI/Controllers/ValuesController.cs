using DataAcess;
using IdentityManagerAPI.ControllerService.IControllerService;
using IdentityManagerAPI.Repos.IRepos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class WorkerController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWorkerFacadeService _workerService;
    private readonly IUnitOfWork _unitOfWork;

    public WorkerController(IWorkerFacadeService workerService, ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _workerService = workerService;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("By_category")]
    public async Task<IActionResult> GetWorkersByCategory(string category)
    {
        var workers = await _workerService.GetWorkersByCategory(category);
        return workers.Any() ? Ok(workers) : NotFound("No worker found in the category");
    }

    [HttpGet("Top_Worker")]
    public async Task<IActionResult> GetTopRatedWorkers(int count)
    {
        var workers = await _workerService.GetTopRatedWorkers(count);
        return Ok(workers);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddWorker([FromBody] Worker worker)
    {
        if (worker == null)
            return BadRequest("Invalid data.");

        await _unitOfWork.Workers.AddAsync(worker);
        await _unitOfWork.SaveAsync();
        return Ok(worker);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorker(int id)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);
        return worker != null ? Ok(worker) : NotFound();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllWorkers()
    {
        var workers = await _context.Workers.ToListAsync();
        return Ok(workers);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateWorker(int id, [FromBody] Worker updatedWorker)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);
        if (worker == null)
            return NotFound();

        worker.Name = updatedWorker.Name;
        worker.Rating = updatedWorker.Rating;
        await _unitOfWork.SaveAsync();
        return Ok(worker);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteWorker(int id)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);
        if (worker == null)
            return NotFound();

        await _unitOfWork.Workers.DeleteAsync(worker);
        await _unitOfWork.SaveAsync();
        return Ok("Worker deleted successfully.");
    }
}
