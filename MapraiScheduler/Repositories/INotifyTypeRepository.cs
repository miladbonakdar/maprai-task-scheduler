using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;

namespace MapraiScheduler.Repositories
{
    public interface INotifyTypeRepository
    {
        IQueryable<NotifyType> GetAll();

        Task<NotifyType> Get(string uniqueName);
    }
}