using MapraiScheduler.Models.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public interface INotifyRepository
    {
        Task Insert(Notify notify);

        Task AddRange(List<Notify> notifyRange);

        Task<bool> ChechIfNotifyDoesNotExist(long invalidPhonePhoneId, int timeStampMiniute);
    }
}