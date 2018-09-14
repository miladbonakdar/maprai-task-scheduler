using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public class ProjectReportRepository : IProjectReportRepository
    {
        private readonly MapRaiContex _mapRaiContex;

        public ProjectReportRepository(MapRaiContex mapRaiContex)
        {
            _mapRaiContex = mapRaiContex;
        }
    }
}