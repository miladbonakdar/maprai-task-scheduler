using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;

namespace MapraiScheduler.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();

        Task<List<User>> GetRelatedUsers(long? organizationId);
    }
}