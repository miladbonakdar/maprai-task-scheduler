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