using MapraiScheduler.Exception;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Repositories.Contracts;
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

        public IQueryable<NotifyType> GetAll() => _mapRaiContex.NotifyTypes;

        //TESTED AND IT IS FINE
        public async Task<NotifyType> GetAsync(string uniqueName)
        {
            try
            {
                return await GetAll().FirstOrDefaultAsync(item => item.UniqueName == uniqueName);
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "NotifyTypeRepository GetAsync");
            }
        }
    }
}