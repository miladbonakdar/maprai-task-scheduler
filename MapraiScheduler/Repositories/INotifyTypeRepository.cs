using MapraiScheduler.Models.Database;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public interface INotifyTypeRepository
    {
        IQueryable<NotifyType> GetAll();

        Task<NotifyType> Get(string uniqueName);
    }
}