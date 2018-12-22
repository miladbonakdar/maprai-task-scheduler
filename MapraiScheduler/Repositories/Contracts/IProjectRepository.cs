using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories.Contracts
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetValidActiveProjects(string uniqueName);

        Task<List<Project>> GetEmptyReportProjects();

        Task<List<Project>> GetEmptyDamageReportProjects(int level);

        IQueryable<Project> GetAll();

        Task<List<NotifyDTO>> GetInvalidProjectsNotifies(List<long> projectIds, string notifyUniqueName);

        Task StopProjectsAsync(List<Project> invalidProjects);
    }
}