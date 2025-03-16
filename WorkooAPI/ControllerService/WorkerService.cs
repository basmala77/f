using DataAcess;
using IdentityManager.Services.ControllerService.IControllerService;
using IdentityManagerAPI.ControllerService.IControllerService;
using IdentityManagerAPI.Repos.IRepos;

namespace IdentityManagerAPI.ControllerService
{/// <summary>
 /// Facade class that manages worker-related operations 
 /// and interacts with multiple services like NotificationService and Repository.
 /// </summary>
    public class WorkerFacadeService : IWorkerFacadeService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IGeolocationService _geolocationService;

        public WorkerFacadeService(IWorkerRepository workerRepository, IGeolocationService geolocationService)
        {
            _workerRepository = workerRepository;
            _geolocationService = geolocationService;
        }
        public async Task<IEnumerable<Worker>> GetWorkersByCategory(string category)
        {
            return await _workerRepository.GetWorkerByCategory(category);
        }

        public async Task<IEnumerable<Worker>> GetTopRatedWorkers(int count)
        {
            return await _workerRepository.GetTopRatedWorkers(count);
        }

        public async Task<IEnumerable<Worker>> GetWorkersByCategoryWithNear(string category, double userLat, double userLon)
        {
            var workers = await _workerRepository.GetWorkerByCategory(category);

           
            var nearbyWorkers = workers.Where(worker =>
                _geolocationService.CalculateDistance(Convert.ToDouble(worker.Lat), Convert.ToDouble(worker.Long), userLat, userLon) <= 5
            ).ToList();

            return nearbyWorkers;
        }
    }
}
