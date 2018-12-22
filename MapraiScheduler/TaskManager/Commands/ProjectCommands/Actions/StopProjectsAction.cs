using MapraiScheduler.Models.Database;
using MapraiScheduler.Repositories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapraiScheduler.TaskManager.Commands.ProjectCommands.Actions
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

        public async Task<IAction> RunAsync()
        {
            await _projectRepository.StopProjectsAsync(InvalidProjects);
            return this;
        }
    }
}