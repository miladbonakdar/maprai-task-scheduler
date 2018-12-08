using MapraiScheduler.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public class NotifyTypeRepository : INotifyTypeRepository
    {
        private readonly MapRaiContex _mapRaiContex;

        public NotifyTypeRepository(MapRaiContex mapRaiContex)
        {
            _mapRaiContex = mapRaiContex;
        }

        public IQueryable<NotifyType> GetAll()
        {
            return _mapRaiContex.NotifyTypes;
        }

        public async Task<NotifyType> Get(string uniqueName)
        {
            return await GetAll().FirstOrDefaultAsync(item => item.UniqueName == uniqueName);
        }
    }
}