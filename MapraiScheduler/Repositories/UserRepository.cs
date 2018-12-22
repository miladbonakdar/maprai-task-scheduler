using MapraiScheduler.Exception;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MapRaiContex _mapRaiContex;

        public UserRepository(MapRaiContex mapRaiContex)
        {
            _mapRaiContex = mapRaiContex;
        }

        //TESTED AND IT IS WOTKING FINE
        public Task<List<User>> GetRelatedUsers(long organizationId)
        {
            try
            {
                return GetAll().Where(item => item.OrganizationID == organizationId || item.OrganizationID == 1).ToListAsync();
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "GetRelatedUsers");
            }
        }

        public IQueryable<User> GetAll()
        {
            return _mapRaiContex.Users;
        }
    }
}