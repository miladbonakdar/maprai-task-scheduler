using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public interface IPhoneRepository
    {
        Task<List<NotifyDTO>> GetInvalidPhonesNotifies(List<long> phoneIds, string notifyUniqueName);

        Task<List<Phone>> GetInvalidPhones(int threshHold);
    }
}