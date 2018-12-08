using MapraiScheduler.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly MapRaiContex _mapRaiContex;

        public NotifyRepository(MapRaiContex mapRaiContex)
        {
            _mapRaiContex = mapRaiContex;
        }

        public async Task Insert(Notify notify)
        {
            _mapRaiContex.Notifiers.Add(notify);
            await _mapRaiContex.SaveChangesAsync();
        }

        public async Task AddRange(List<Notify> notifyRange)
        {
            _mapRaiContex.Notifiers.AddRange(notifyRange);
            await _mapRaiContex.SaveChangesAsync();
        }

        public async Task<bool> ChechIfNotifyDoesNotExist(long invalidPhonePhoneId, int timeStampMiniute)
        {
            var lastNotify = await _mapRaiContex.Notifiers.Where(not =>
                not.PhoneID != null && not.PhoneID == invalidPhonePhoneId &&
                (DateTime.Now - not.CreationDate).Minutes < timeStampMiniute).FirstOrDefaultAsync();
            return lastNotify == null;
        }
    }
}