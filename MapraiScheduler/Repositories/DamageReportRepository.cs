using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public class DamageReportRepository : IDamageReportRepository
    {
        private readonly MapRaiContex _mapRaiContex;

        public DamageReportRepository(MapRaiContex mapRaiContex)
        {
            _mapRaiContex = mapRaiContex;
        }
    }
}