using DataAcess;
using System.Threading.Tasks;

namespace IdentityManagerAPI.ControllerService.IControllerService
{
    public interface IWorkerFacadeService
    {
        Task<IEnumerable<Worker>> GetWorkersByCategory(string category);
        Task<IEnumerable<Worker>> GetTopRatedWorkers(int count);
        Task<IEnumerable<Worker>> GetWorkersByCategoryWithNear(string category, double userLat, double userLon);

    }
}
