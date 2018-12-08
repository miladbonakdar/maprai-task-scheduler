using MapraiScheduler.Models.Database;
using MapraiScheduler.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapraiScheduler.TaskManager.Commands.Action
{
    public class StopProjectsAction : IStopProjectsAction
    {
        private List<Project> InvalidProjects { set; get; }
        private readonly IProjectRepository _projectRepository;

        public StopProjectsAction(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IAction SetDatas(object dataToSet)
        {
            InvalidProjects = (List<Project>)dataToSet;
            return this;
        }

        public async Task<IAction> Run()
        {
            _projectRepository.StopProjects(InvalidProjects);
            return this;
        }
    }
}