using MapraiScheduler.Models.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories.Contracts
{
    public interface INotifyRepository
    {
        Task Insert(Notify notify);

        Task AddRange(List<Notify> notifyRange);

        Task<bool> ChechIfNotifyDoesNotExist(long invalidPhonePhoneId, int timeStampMiniute, long notifyType = 4);
    }
}