using MapraiScheduler.Models.Database;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories.Contracts
{
    public interface INotifyTypeRepository
    {
        IQueryable<NotifyType> GetAll();

        Task<NotifyType> GetAsync(string uniqueName);
    }
}