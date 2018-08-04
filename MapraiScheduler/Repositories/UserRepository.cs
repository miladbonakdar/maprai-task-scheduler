using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MapraiScheduler.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MapRaiContex _mapRaiContex;

        public UserRepository(MapRaiContex mapRaiContex)
        {
            _mapRaiContex = mapRaiContex;
        }

        public Task<List<User>> GetRelatedUsers(long? organizationId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            return GetAll().Where(item => item.OrganizationID == organizationId.Value || item.OrganizationID == 1).ToListAsync();
        }

        public IQueryable<User> GetAll()
        {
            return _mapRaiContex.Users;
        }
    }
}